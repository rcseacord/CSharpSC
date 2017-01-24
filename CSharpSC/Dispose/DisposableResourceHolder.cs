using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SecureCSharp
{
  public class DisposableResourceHolder : IDisposable
  {
    private DisposableStreamResource2 resource; // handle to a resource
    public DisposableResourceHolder()
    {
      resource = new DisposableStreamResource2(@"..\..\file.txt");
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~DisposableResourceHolder() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion

  }

  class DisposableResourceHolderSubclass : 
    DisposableResourceHolder
  {
    private bool disposedValue = false; // To detect redundant calls

    // Instantiate a SafeHandle instance.
    SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

    // Protected implementation of Dispose pattern.
    protected override void Dispose(bool disposing)
    {
      if (disposedValue)
        return;

      if (disposing)
      {
        handle.Dispose();
        // Free any other managed objects here.
      }
      // Free any unmanaged objects here.

      disposedValue = true;
      // Call base class implementation.
      base.Dispose(disposing);
    }
  }

}
