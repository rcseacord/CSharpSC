using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace SecureCSharp
{
  public class DisposableStreamResource2 : DisposableStreamResource
  {
    // Define additional constants.
    internal const uint GENERIC_WRITE = 0x40000000;
    internal const uint OPEN_ALWAYS = 4;

    internal static class UnsafeNativeMethods
    {
      // Define additional APIs.
      [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern Boolean WriteFile(
        SafeFileHandle safeHandle, string lpBuffer,
        uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
        [In] ref NativeOverlapped lpOverlapped
      );
    }

    // Define locals.
    private bool disposed = false;
    private string filename;
    private bool created = false;
    private SafeFileHandle safeFileHandle;
    private NativeOverlapped nativeOverlapped;

    public DisposableStreamResource2(string fileName) : base(fileName)
    {
      this.filename = fileName;
    }

    public bool WriteFileInfo()
    {
      if (!created)
      {
         safeFileHandle = NativeMethods.CreateFile(
          @".\FileInfo.txt", GENERIC_WRITE, 0, IntPtr.Zero, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero
        );
        if (safeFileHandle.IsInvalid)
        {
          throw new IOException(
          String.Format(CultureInfo.CurrentCulture, "Unable to create output file."),
          new System.ComponentModel.Win32Exception());  // calls Marshal.GetLastWin32Error
        }
        nativeOverlapped = new NativeOverlapped();
        created = true;
      }

      string output = String.Format(CultureInfo.CurrentCulture, "{0}: {1:N0} bytes\n", filename, Size);
      return UnsafeNativeMethods.WriteFile(safeFileHandle, output, (uint)output.Length, out uint bytesWritten, ref nativeOverlapped);
    }

    protected new virtual void Dispose(bool disposing)
    {
      if (disposed) return;

      // Release any managed resources here.
      if (disposing)
        safeFileHandle.Dispose();

      disposed = true;  

      // Release any unmanaged resources not wrapped by safe handles here.

      // Call the base class implementation.
      base.Dispose(true);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    public static void Main()
    {
      using (DisposableStreamResource2 dsr2 = new DisposableStreamResource2(@"..\..\file.txt"))
      {
        Console.WriteLine("File Size = " + dsr2.Size);
        // Keep the console window open in debug mode.
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
      }
    }
  }
}