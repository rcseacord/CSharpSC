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

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{

  public static class ExceptionExtension
  {
    public static bool LogException(this Exception exception)
    {
      Console.Error.WriteLine("Exceptions happen: {0}", exception);
      return false;
    }
  }
  public class Dimensions
  {
    private int _l, _w, _h;
    private const int Pad = 2;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "w")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h")]
    public Dimensions(int l, int w, int h)
    {
      _l = l;
      _w = w;
      _h = h;
    }

    private int GetVolumePackage(uint weight)
    {
      _l += Pad;
      _w += Pad;
      _h += Pad;
      if (weight > 20)
      {
        throw new ArgumentOutOfRangeException(message: "Excessive weight", paramName: nameof(weight));
      }
      int volume = _l * _w * _h; // 12 * 12 * 12 = 1728

      // Revert fields
      _l -= Pad;
      _w -= Pad;
      _h -= Pad; 

      return volume;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    public static void Main()
    {
      bool status = true;
      var d = new Dimensions(10, 10, 10);
      try
      {
        Console.WriteLine(d.GetVolumePackage(21));
      }
      catch (ArgumentOutOfRangeException e)
      {
        status = e.LogException();
      }

      try
      {
        Console.WriteLine(d.GetVolumePackage(19)); // Prints 2744 instead of 1728
      }
      catch (ArgumentOutOfRangeException e)
      {
        status = e.LogException();
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
      Environment.Exit(status ? 0 : 1);
    }  // end Main
  } // end class Dimensions
} // end NameSpace






































/*
private int GetVolumePackage(uint weight) {
  if (weight > 20) {
    throw new ArgumentOutOfRangeException(message: "Weight out of range", paramName: nameof(weight));
  }
  return (_l + Pad) * (_w + Pad) * (_h + Pad);
}
*/
