using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{
  public class ComplexWindow : IDisposable
  {
    private SafeMyResourceHandle handle; // pointer for a resource
    private Component component; // other resource you use

    private bool disposed = false;

    public ComplexWindow()
    {
      handle = CreateWindow(
          "MyClass", "Test Window",
          0, 50, 50, 500, 900,
          IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

      component = new Component();
    }

    // implements IDisposable
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        // if this is a dispose call dispose on all state you
        // hold, and take yourself off the Finalization queue.

        if (disposing)
        {
          if (handle != null)
            handle.Dispose();

          if (component != null)
          {
            component.Dispose();
            component = null;
          }
        }

        // free your own state (unmanaged objects)
        AdditionalCleanup();

        this.disposed = true;
      }
    }

    // finalizer simply calls Dispose(false)
    ~ComplexWindow()
    {
      Dispose(false);
    }

    // some custom cleanup logic
    private void AdditionalCleanup()
    {
      // this method should not allocate or take locks, unless
      // absolutely needed for security or correctness reasons.
      // since it is called during finalization, it is subject to
      // all of the restrictions on finalizers above.
    }

    // whenever you do something with this class, check to see if the
    // state is disposed, if so, throw this exception.
    public void ShowWindow()
    {
      if (this.disposed)
      {
        throw new ObjectDisposedException("");
      }

      // otherwise, do work
    }

    [DllImport("user32.dll", SetLastError = true,
      CharSet = CharSet.Auto, BestFitMapping = false)]
    private static extern SafeMyResourceHandle CreateWindow(
      string lpClassName, string lpWindowName, int dwStyle,
      int x, int y, int nWidth, int nHeight, IntPtr hwndParent,
      IntPtr Menu, IntPtr hInstance, IntPtr lpParam);

    internal sealed class SafeMyResourceHandle : SafeHandle
    {
      private HandleRef href;

      // called by P/Invoke when returning SafeHandles
      private SafeMyResourceHandle() : base(IntPtr.Zero, true)
      {
      }

      // no need to provide a finalizer - SafeHandle's critical
      // finalizer will call ReleaseHandle for you.

      public override bool IsInvalid
      {
        get { return handle == IntPtr.Zero; }
      }

      override protected bool ReleaseHandle()
      {
        // this method is a constrained execution region, and
        // *must* not allocate

        return DeleteObject(href);
      }

      [DllImport("gdi32.dll")]
      [SuppressUnmanagedCodeSecurityAttribute()]
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      private static extern bool DeleteObject(HandleRef hObject);

      [DllImport("kernel32")]
      internal static extern SafeMyResourceHandle CreateHandle(int someState);
    }
  }

  // derived class
  public class MyComplexWindow : ComplexWindow
  {
    private Component myComponent; // other resource you use

    private bool disposed = false;

    public MyComplexWindow()
    {
      myComponent = new Component();
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          // free any disposable resources you own
          if (myComponent != null)
            myComponent.Dispose();

          this.disposed = true;
        }

        // perform any custom clean-up operations
        // such as flushing the stream
      }

      base.Dispose(disposing);
    }

    static void Main()
    {
      MyComplexWindow mcw = new MyComplexWindow();
      Console.WriteLine(mcw.ToString());
      mcw.Dispose();
    }
  }
}