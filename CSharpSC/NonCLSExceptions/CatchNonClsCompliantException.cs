// Build with the following commands:
// Uncomment out line 17
// C:\Windows\Microsoft.NET\Framework64\v4.0.30319\ilasm.exe /dll ThrowNonClsCompliantException.il
// C:\Users\rseacord\Documents\Visual Studio 2015\Projects\CSharpSC\NonCLSExceptions>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc /r:ThrowNonClsCompliantException.dll CatchNonClsCompliantException.cs

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
using System.Runtime.CompilerServices;

[assembly: RuntimeCompatibilityAttribute(WrapNonExceptionThrows = false)]
namespace SecureCSharp
{

  struct HandlesExceptions
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    static void CatchAllExceptions()
    {
      try
      {
        //        ThrowsExceptions.ThrowNonClsException();
      }
      catch (Exception e)
      {
        RuntimeWrappedException rwe = e as RuntimeWrappedException;
        Object o = rwe.WrappedException as Object;
        if (o != null)
        {
          Console.WriteLine(o.ToString());
        }
        else
        {
          // Handle other System.Exception types.  
        }

        // Remove some permission.
        Console.Error.WriteLine("CLS compliant exception: {0}", e.ToString());
      }
      catch
      {
        // Remove the same permission as above.
        Console.WriteLine("Non-CLS compliant exception caught.");
      }
    }

    static void Main()
    {
      HandlesExceptions.CatchAllExceptions();
    }
  }
}
