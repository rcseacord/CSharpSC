﻿// The MIT License (MIT)
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
using System.IO;

[assembly: CLSCompliant(true)]
namespace DisposableField
{
    // Implement the IDisposable interface and dispose of unmanaged resources. 
    public class FileStreamHolder
    {
      private readonly FileStream _newFile;
        public FileStreamHolder()
        {
            _newFile = new FileStream(@"..\..\file.txt", FileMode.Open);
        }
        public bool IsInvalid => _newFile == null;

      private static void Main()
        {
            var fsh = new FileStreamHolder();
            Console.WriteLine(fsh.IsInvalid ? "Invalid file stream" : fsh.ToString());

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

    }  // end class FileStreamHolder
} // end namespace DisposableField






















































//namespace DisposableField
//{
//  // Implement the IDisposable interface and dispose of unmanaged resources. 
//  public class FileStreamHolder : IDisposable
//  {
//    private readonly FileStream _newFile;
//    public FileStreamHolder()
//    {
//      _newFile = new FileStream(@"..\..\file.txt", FileMode.Open);
//    }
//    public bool IsInvalid => _newFile == null;

//    private static void Main()
//    {
//      var fsh = new FileStreamHolder();
//      try
//      {
//        Console.WriteLine(fsh.IsInvalid ? "Invalid file stream" : fsh.ToString());
//      }
//      finally
//      {
//        fsh.Dispose();
//      }
//      // Keep the console window open in debug mode.
//      Console.WriteLine("Press any key to exit.");
//      Console.ReadKey();
//    }

//    #region IDisposable Support
//    private bool _disposedValue; // To detect redundant calls

//    protected virtual void Dispose(bool disposing)
//    {
//      if (_disposedValue) return;
//      if (disposing)
//      {
//        _newFile.Close();
//      }
//      _disposedValue = true;
//    }

//    // Because the class does not directly own any unmanaged resources, 
//    // it does NOT implement a finalizer.

//    // This code added to correctly implement the disposable pattern.
//    public void Dispose()
//    {
//      Dispose(true);
//      GC.SuppressFinalize(this);
//    }
//    #endregion
//  }  // end class FileStreamHolder
//} // end namespace DisposableField