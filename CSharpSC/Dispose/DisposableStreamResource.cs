using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{
  using Microsoft.Win32.SafeHandles;
  using System;
  using System.IO;
  using System.Runtime.InteropServices;

  public class DisposableStreamResource : IDisposable
  {
    // Define constants.
    internal const uint GENERIC_READ = 0x80000000;
    internal const uint FILE_SHARE_READ = 0x00000001;
    internal const uint OPEN_EXISTING = 3;
    internal const uint FILE_ATTRIBUTE_NORMAL = 0x80;
    internal const int INVALID_FILE_SIZE = unchecked((int)0xFFFFFFFF);

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
    private bool disposed = false;
    private SafeFileHandle safeFileHandle;
    private long bufferSize;
    private int upperWord;

    public DisposableStreamResource(string fileName)
    {
      if (String.IsNullOrWhiteSpace(fileName))
      {
        throw new ArgumentException(message: "The filename cannot be null, empty, or whitespace.", paramName: nameof(fileName));
      }

      safeFileHandle = NativeMethods.CreateFile(fileName, GENERIC_READ, FILE_SHARE_READ,
                                 IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL,
                                 IntPtr.Zero);
      if (safeFileHandle.IsInvalid)
      {
        throw new FileNotFoundException(
          String.Format(CultureInfo.CurrentCulture, "Cannot open '{0}'", fileName),
          new System.ComponentModel.Win32Exception());  // calls Marshal.GetLastWin32Error
      }

      // Get file size.
      bufferSize = NativeMethods.GetFileSize(this.safeFileHandle, out upperWord);
      if (bufferSize == INVALID_FILE_SIZE)
        bufferSize = -1;
      else if (upperWord > 0)
        bufferSize = (((long)upperWord) << 32) + bufferSize;
    }

    public long Size
    { get { return bufferSize; } }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposed) return;

      // Dispose of managed resources here.
      if (disposing) safeFileHandle?.Dispose();

      // Dispose of any unmanaged resources not wrapped in safe handles.

      disposed = true;  
    }
  } // end public class DisposableStreamResource
} // end namespace SecureCSharp