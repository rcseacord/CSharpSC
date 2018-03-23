using System;

namespace DerivedDisposable
{
  internal class BaseDisposable : IDisposable
  {
    // Flag: Has Dispose already been called?
    private bool _disposed = false;

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
      Console.WriteLine("BaseDisposable:Dispose disposing = {0}", disposing);
      if (_disposed)
        return;

      if (disposing)
      {
        // Free any other managed objects here.
      }

      // Free any unmanaged objects here.
      _disposed = true;
    }

    ~BaseDisposable()
    {
      Console.WriteLine("~BaseDisposable");
      Dispose(false);
    }
  }

  internal class DerivedDisposable : BaseDisposable
  {
    // Flag: Has Dispose already been called?
    private bool _disposed = false;

    // Protected implementation of Dispose pattern.
    protected override void Dispose(bool disposing)
    {
      Console.WriteLine("DerivedDisposable:Dispose disposing = {0}", disposing);
      if (_disposed)
        return;

      if (disposing)
      {
        // Free any other managed objects here.
      }

      // Free any unmanaged objects here.
      _disposed = true;

      // Call the base class implementation.
      base.Dispose(disposing);
    }

    ~DerivedDisposable()
    {
      Console.WriteLine("~DerivedDisposable");
      Dispose(false);
    }
  }

  public class Program
  {
    private static void F()
    {
      DerivedDisposable dd = new DerivedDisposable();
    }
    private static void Main()
    {
      F();
      GC.Collect();
      GC.WaitForPendingFinalizers();

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }

  }  // end class FileStreamHolder

}
