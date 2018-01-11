﻿// The MIT License (MIT)
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
using System.Security;
using Microsoft.Win32.SafeHandles;

[assembly: CLSCompliant(true)]
namespace Dispose
{
    public class DisposableStreamResource : IDisposable
  {
    // Define constants.
    internal const uint GenericRead = 0x80000000;
    internal const uint FileShareRead = 0x00000001;
    internal const uint OpenExisting = 3;
    internal const uint FileAttributeNormal = 0x80;
    internal const int InvalidFileSize = unchecked((int)0xFFFFFFFF);

    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
      // Define Windows APIs.
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      internal static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess,
          uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
          uint dwFlagsAndAttributes, IntPtr hTemplateFile
      );

      [DllImport("kernel32.dll")]
      internal static extern int GetFileSize(SafeFileHandle hFile, out int lpFileSizeHigh);
    }
    // Define locals.
    private bool _disposed;
    private readonly SafeFileHandle _safeFileHandle;

    public DisposableStreamResource(string fileName)
    {
      if (String.IsNullOrWhiteSpace(fileName))
      {
        throw new ArgumentException(message: "The filename cannot be null, empty, or whitespace.", paramName: nameof(fileName));
      }

      _safeFileHandle = NativeMethods.CreateFile(fileName, GenericRead, FileShareRead,
                                 IntPtr.Zero, OpenExisting, FileAttributeNormal,
                                 IntPtr.Zero);
      if (_safeFileHandle.IsInvalid)
      {
        throw new FileNotFoundException(
          String.Format(CultureInfo.CurrentCulture, "Cannot open '{0}'", fileName),
          new System.ComponentModel.Win32Exception());  // calls Marshal.GetLastWin32Error
      }

      // Get file size.
      Size = NativeMethods.GetFileSize(this._safeFileHandle, out var upperWord);
      if (Size == InvalidFileSize)
        Size = -1;
      else if (upperWord > 0)
        Size = (((long)upperWord) << 32) + Size;
    }

    public long Size { get; }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (_disposed) return;

      // Dispose of managed resources here.
      if (disposing) _safeFileHandle?.Dispose();

      // Dispose of any unmanaged resources not wrapped in safe handles.

      _disposed = true;  
    }
  } // end public class DisposableStreamResource
} // end namespace SecureCSharp