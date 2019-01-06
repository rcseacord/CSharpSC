//namespace SafeHandleDemo
//{
//  // SafeHandle demo, V2 version.  This example shows the best state of
//  // the art as of v2 (using SafeHandle) for writing a class that wraps
//  // an OS handle.  This sample shows how to harden an existing class library
//  // to correctly free resources even in the presence of async exceptions.
//  // This tries to mimic a real library as close as possible, to show you
//  // precisely what design & implementation considerations may arise.
//  // Note the P/Invoke method signatures, and the Dispose & lack of any
//  // finalizer below.  Also, below I have a primitive fault injection
//  // harness for testing this code, to demonstrate that the handle leak
//  // has been fixed.

//  using System;
//  using System.Runtime.InteropServices;
//  using System.IO;
//  using System.ComponentModel;
//  using System.Security.Permissions;
//  using System.Security;
//  using System.Threading;
//  using Microsoft.Win32.SafeHandles;
//  using System.Runtime.ConstrainedExecution;

//  namespace SafeHandleDemoV2
//  {
//    // If this class is ever public, ensure the callers have unmanaged
//    // code permission.

//    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]

//    internal class MySafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
//    {
//      // Create a SafeHandle, informing the base class that this SafeHandle
//      // instance "owns" the handle, and therefore SafeHandle should call
//      // our ReleaseHandle method when the SafeHandle is no longer in use.
//      // This method is called only via P/Invoke.

//      private MySafeFileHandle() : base(true)
//      {

//      }

//      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]

//      protected override bool ReleaseHandle()
//      {
//        // Here, we must obey all rules for constrained execution regions.
//        return UnsafeNativeMethods.CloseHandle(handle);

//        // If ReleaseHandle failed, it can be reported via the
//        // "releaseHandleFailed" managed debugging assistant (MDA).  This
//        // MDA is disabled by default, but can be enabled in a debugger
//        // or during testing to diagnose handle corruption problems.
//        // We do not throw an exception because most code could not recover
//        // from the problem.
//      }
//    }

//    // We've reviewed this class & take responsibility for security reviews, etc.

//    [SuppressUnmanagedCodeSecurity]
//    internal static class UnsafeNativeMethods
//    {
//      // Win32 constants for accessing files
//      internal const int GenericRead = unchecked((int)0x80000000);

//      // Allocate a file object in the kernel, then return a handle to it.

//      [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
//      internal static extern MySafeFileHandle CreateFile(string fileName,
//         int dwDesiredAccess, FileShare dwShareMode,
//         IntPtr securityAttrsMustBeZero, FileMode dwCreationDisposition,
//         int dwFlagsAndAttributes, IntPtr hTemplateFileMustBeZero);

//      // Use the file handle
//      [DllImport("kernel32", SetLastError = true)]

//      internal static extern int ReadFile(MySafeFileHandle handle, byte[] bytes,
//         int numBytesToRead, out int numBytesRead, IntPtr overlappedMustBeZero);

//      // Free the kernel's file object (close the file)
//      [DllImport("kernel32", SetLastError = true)]
//      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
//      internal static extern bool CloseHandle(IntPtr handle);
//    }

//    // This is a sample class that accesses an OS resource and implements
//    // IDisposable.  This is useful to show the types of transformation that
//    // are required to make your resource-wrapping classes more resilient.
//    // Note the Dispose & finalizer implementations.  Consider this a very
//    // good simulation of System.IO.FileStream.
//    public class MyFileReader : IDisposable
//    {
//      // _handle is set to null to indicate that we've disposed of this instance.
//      private readonly MySafeFileHandle _handle;

//      public MyFileReader(string fileName)
//      {
//        // Security permission check
//        string fullPath = Path.GetFullPath(fileName);
//        new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();

//        // Open the file, and save the file handle in _handle
//        // Note that the most optimized code turns into two processor
//        // instructions: 1) a call, and 2) moving the return value into
//        // the _handle field.  With SafeHandle, the CLR's Platform Invoke
//        // marshaling layer will store the handle into the SafeHandle
//        // object for us in an atomic fashion.  We still have the problem
//        // that the SafeHandle object may not be stored in _handle, but
//        // the real OS handle value has been safely stored in a critical
//        // finalizable object, ensuring we won't leak the handle, even if
//        // we get an async exception here.

//        MySafeFileHandle tmpHandle = UnsafeNativeMethods.CreateFile(fileName, UnsafeNativeMethods.GenericRead,
//            FileShare.Read, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

//        // An async exception here will cause us to run our finalizer with
//        // a null _handle, but MySafeFileHandle's ReleaseHandle code will
//        // run to free the handle. 
//        // This call to Sleep will help trigger a race when run from our
//        // fault injection harness (below in Main).  This race won’t cause
//        // a handle leak because the handle is already stored in a
//        // SafeHandle instance.  Critical finalization then guarantees
//        // that we can free the handle, even during a rude AppDomain unload.
//        // I'll leave in this Sleep for comparison with the v1.1 version,
//        // which would leak the handle under stress.
//        // Thread.Sleep(500);
//        _handle = tmpHandle;  // Makes _handle point to a critical finalizable object.

//        // Ensure we successfully opened the file.
//        if (_handle.IsInvalid)
//          throw new Win32Exception(Marshal.GetLastWin32Error(), fileName);
//      }

//      public void Dispose()  // Follow the Dispose pattern - public nonvirtual
//      {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//      }

//      // We don't need a finalizer.  ~MyFileReader has been removed.
//      // The finalizer on SafeHandle will clean up the MySafeFileHandle
//      // instance, if it hasn't already been disposed.

//      // There may be a need for a subclass to introduce a finalizer,
//      // so we will still properly implement the Dispose pattern, with a
//      // protected Dispose(bool).

//      protected virtual void Dispose(bool disposing)
//      {
//        // Note there are three interesting states here:
//        // 1) CreateFile failed, _handle contains an invalid handle
//        // 2) We called Dispose already, _handle is closed.
//        // 3) _handle is null, due to an async exception before
//        //    calling CreateFile. The finalizer runs if the .ctor fails!
//        if (_handle != null && !_handle.IsInvalid)
//        {
//          // Free the handle
//          _handle.Dispose();
//        }

//        // SafeHandle records the fact that we've called Dispose.

//      }

//      public byte[] ReadContents(int length)
//      {

//        if (_handle.IsInvalid)  // Disposed?
//          throw new ObjectDisposedException("FileReader is closed");

//        // This is just sample code & won't work for all files.
//        var bytes = new byte[length];
//        int r = UnsafeNativeMethods.ReadFile(_handle, bytes, length, out var numRead, IntPtr.Zero);

//        // Since we removed MyFileReader's finalizer, we no longer need to
//        // call GC.KeepAlive here.  P/Invoke will keep the SafeHandle
//        // instance alive for the duration of the call.

//        if (r == 0)
//          throw new Win32Exception(Marshal.GetLastWin32Error());

//        if (numRead >= length) return bytes;
//        var newBytes = new byte[numRead];
//        Array.Copy(bytes, newBytes, numRead);
//        bytes = newBytes;

//        return bytes;
//      }
//    }

//    internal static class Program
//    {
//      // For building a fault injection tester (initialized to false)
//      private static bool _printToConsole;
//      private static bool _workerStarted;

//      private static void Usage()
//      {
//        Console.WriteLine("Usage:");
//        Console.WriteLine("HexViewer <fileName> [-fault]");
//        Console.WriteLine(" -fault   Run the hex viewer over & over in a loop, injecting faults.");
//      }

//      private static void ViewInHex(object fileName)
//      {
//        _workerStarted = true;
//        byte[] bytes;

//        using (var reader = new MyFileReader((string)fileName))
//        {
//          bytes = reader.ReadContents(20);
//        }  // Using block calls Dispose() for us here

//        if (!_printToConsole) return;
//        int printNBytes = Math.Min(20, bytes.Length);
//        Console.WriteLine("First {0} bytes of {1} in hex", printNBytes, fileName);

//        for (int i = 0; i < printNBytes; i++)
//          Console.Write("{0:x} ", bytes[i]);

//        Console.WriteLine();
//      }

//      private static void Main(string[] args)
//      {
//        if (args.Length == 0 || args.Length > 2 || args[0] == "-?" || args[0] == "/?")
//        {
//          Usage();
//          return;
//        }

//        string fileName = args[0];
//        bool injectFaultMode = args.Length > 1;

//        if (!injectFaultMode)
//        {
//          _printToConsole = true;
//          ViewInHex(fileName);
//        }
//        else
//        {
//          Console.WriteLine("Injecting faults - watch handle count in perfmon (press Ctrl-C when done)");
//          int numIterations = 0;
//          while (true)
//          {
//            _workerStarted = false;
//            var t = new Thread(ViewInHex);
//            t.Start(fileName);
//            Thread.Sleep(1);
//            while (!_workerStarted)
//            {
//              Thread.Sleep(0);
//            }

//            t.Abort();  // Normal apps shouldn't do this, of course.
//            numIterations++;

//            if (numIterations % 10 == 0)
//              GC.Collect();

//            if (numIterations % 10000 == 0)
//              Console.WriteLine(numIterations);

//          }  // end while (true)
//        } // end else
//      }  // end Main()

//    } // end class Program
//  }  // end namespace
//}  // end namespace
