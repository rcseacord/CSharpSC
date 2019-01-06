// The MIT License (MIT)
// 
// Copyright (c) 2018 Robert C. Seacord
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
