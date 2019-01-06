// The MIT License (MIT)
// 
// Copyright (c) 2017 Robert C. Seacord
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Dispose
{
  public class DisposableStreamResource2 : DisposableStreamResource
  {
    // Define additional constants.
    internal const uint GenericWrite = 0x40000000;
    internal const uint OpenAlways = 4;

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
    private bool _disposed;
    private readonly string _filename;
    private bool _created;
    private SafeFileHandle _safeFileHandle;
    private NativeOverlapped _nativeOverlapped;

    public DisposableStreamResource2(string fileName) : base(fileName)
    {
      this._filename = fileName;
    }

    public bool WriteFileInfo()
    {
      if (!_created)
      {
         _safeFileHandle = NativeMethods.CreateFile(
          @".\FileInfo.txt", GenericWrite, 0, IntPtr.Zero, OpenAlways, FileAttributeNormal, IntPtr.Zero
        );
        if (_safeFileHandle.IsInvalid)
        {
          throw new IOException(
          String.Format(CultureInfo.CurrentCulture, "Unable to create output file."),
          new System.ComponentModel.Win32Exception());  // calls Marshal.GetLastWin32Error
        }
        _nativeOverlapped = new NativeOverlapped();
        _created = true;
      }

      string output = String.Format(CultureInfo.CurrentCulture, "{0}: {1:N0} bytes\n", _filename, Size);
      return UnsafeNativeMethods.WriteFile(_safeFileHandle, output, (uint)output.Length, out var bytesWritten, ref _nativeOverlapped);
    }

    protected new virtual void Dispose(bool disposing)
    {
      if (_disposed) return;

      // Release any managed resources here.
      if (disposing)
        _safeFileHandle.Dispose();

      _disposed = true;  

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