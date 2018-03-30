// This example shows the best state of the art as of v1.1 (without SafeHandle) for writing a class that wraps
// an OS handle.  This sample shows how to harden an existing class library
// to correctly free resources even in the presence of async exceptions.
// This tries to mimic a real library as close as possible, to show you
// precisely what design & implementation considerations may arise.
// Note the P/Invoke method signatures, and the Dispose & finalizer below.
// Also, below I have a primitive fault injection harness for testing this
// code, to demonstrate the handle leak in v1.1.

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace SafeHandleDemo
{
  // We've reviewed this class & take responsibility for security reviews, etc.
  [SuppressUnmanagedCodeSecurity]
  internal static class NativeMethods
  {
    // Win32 constants for accessing files
    internal static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
    internal const int GenericRead = unchecked((int)0x80000000);

    // Allocate a file object in the kernel, then return a handle to it.
    [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]

    internal static extern IntPtr CreateFile(string fileName,
       int dwDesiredAccess, FileShare dwShareMode,
       IntPtr securityAttrsMustBeZero, FileMode dwCreationDisposition,
       int dwFlagsAndAttributes, IntPtr hTemplateFileMustBeZero);

    // Use the file handle
    [DllImport("kernel32", SetLastError = true)]

    internal static extern int ReadFile(IntPtr handle, byte[] bytes,
       int numBytesToRead, out int numBytesRead, IntPtr overlappedMustBeZero);

    // Free the kernel's file object (close the file)
    [DllImport("kernel32", SetLastError = true)]

    internal static extern bool CloseHandle(IntPtr handle);
  }

  // This is a sample class that accesses an OS resource and implements
  // IDisposable.  This is useful to show the types of transformation that
  // are required to make your resource-wrapping classes more resilient.
  // Note the Dispose & finalizer implementations.  Consider this a 
  // good simulation of System.IO.FileStream. 

  public class MyFileReader : IDisposable
  {
    // _handle is set to NativeMethods.InvalidHandleValue to indicate
    // that we've disposed of this instance.
    private IntPtr _handle;

    public MyFileReader(string fileName)
    {
      // Security permission check
      string fullPath = Path.GetFullPath(fileName);
      new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();

      // Open the file, and save the file handle in _handle
      // Note that the most optimized code turns into two processor
      // instructions: 1) a call, and 2) moving the return value into
      // the _handle field.
      // We can get an async exception in that window, and leak a handle.
      // The call then mov sequence is explicit here.

      IntPtr tmpHandle = NativeMethods.CreateFile(fileName, NativeMethods.GenericRead,
          FileShare.Read, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

      // Ensure we successfully opened the file.
      if (tmpHandle == NativeMethods.InvalidHandleValue)
        throw new Win32Exception(Marshal.GetLastWin32Error(), fileName);

      // An async exception here will cause us to leak the handle. 
      // I'm sleeping here to explicitly allow an abort to happen in this
      // window, making a rare race condition much easier to hit in our
      // fault injection harness (below in Main)
      Thread.Sleep(500);

      _handle = tmpHandle;  // moves eax into wherever _handle is stored.

    }

    public void Dispose()  // Follow the Dispose pattern - public nonvirtual
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~MyFileReader()
    {
      Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
      // Note there are three interesting states here:
      // 1) CreateFile failed, handle is InvalidHandleValue
      // 2) We called Dispose already
      // 3) _handle is zero-initialized, due to an async exception before
      //    calling CreateFile.  The finalizer runs if the .ctor fails!
      if (_handle != NativeMethods.InvalidHandleValue && _handle != IntPtr.Zero)
      {
        // Free the handle
        bool r = NativeMethods.CloseHandle(_handle);
      }

      // Record the fact that we've closed the handle.
      _handle = NativeMethods.InvalidHandleValue;
    }

    public byte[] ReadContents(int length)
    {
      if (_handle == NativeMethods.InvalidHandleValue)  // Disposed?
        throw new ObjectDisposedException("FileReader is closed");

      // This is just sample code & won't work for all files.
      var bytes = new byte[length];
      int r = NativeMethods.ReadFile(_handle, bytes, length, out var numRead, IntPtr.Zero);
      if (r == 0)
        throw new Win32Exception(Marshal.GetLastWin32Error());

      // The GC can decide to run here, and may determine that this instance
      // is no longer accessible, and it may run our finalizer.  If that
      // happens during the call to ReadFile above, we may be passing in an
      // invalid handle.  In even more rare cases, another thread might use
      // that same handle number for a completely unrelated file!
      // Calling GC.KeepAlive or using HandleRef diligently will help prevent
      // this problem, by telling the GC this instance is still live.
      GC.KeepAlive(this);

      if (numRead >= length) return bytes;
      var newBytes = new byte[numRead];
      Array.Copy(bytes, newBytes, numRead);
      bytes = newBytes;

      return bytes;
    }
  }

  internal static class Program
  {
    // For building a fault injection tester (false by default)
    private static bool _printToConsole;
    private static bool _workerStarted;

    private static void Usage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("HexViewer <fileName> [-fault]");
      Console.WriteLine(" -fault   Run the hex viewer over & over in a loop, injecting faults.");
    }

    private static void ViewInHex(object fileName)
    {
      _workerStarted = true;
      byte[] bytes;
      using (var reader = new MyFileReader((string)fileName))
      {
        bytes = reader.ReadContents(20);
      }  // Using block calls Dispose() for us here

      if (!_printToConsole) return;
      int printNBytes = Math.Min(20, bytes.Length);
      Console.WriteLine("First {0} bytes of {1} in hex", printNBytes, fileName);
      for (int i = 0; i < printNBytes; i++) Console.Write("{0:x} ", bytes[i]);
      Console.WriteLine();
    }

    static void Main(string[] args)
    {
      if (args.Length == 0 || args.Length > 2 ||
          args[0] == "-?" || args[0] == "/?")
      {
        Usage();
        return;
      }

      string fileName = args[0];
      bool injectFaultMode = args.Length > 1;
      if (!injectFaultMode)
      {
        _printToConsole = true;
        ViewInHex(fileName);
      }
      else
      {
        Console.WriteLine("Injecting faults - watch handle count in perfmon (press Ctrl-C when done)");
        int numIterations = 0;
        while (true)
        {
          _workerStarted = false;
          Thread t = new Thread(ViewInHex);
          t.Start(fileName);
          Thread.Sleep(1);
          while (!_workerStarted)
          {
            Thread.Sleep(0);
          }
          t.Abort();  // Normal apps shouldn't do this, of course.
          numIterations++;
          if (numIterations % 10 == 0) GC.Collect();
          if (numIterations % 10000 == 0) Console.WriteLine(numIterations);
        }
      }
    }
  }
}