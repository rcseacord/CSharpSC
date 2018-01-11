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

[assembly: CLSCompliant(true)]
namespace Culture
{
  public static class CultureClub
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    private static void ProcessTag(string tag)
    {
      if (tag.ToUpper().Equals("SCRIPT"))
      {
        Console.WriteLine("script".ToUpper() + " equals " + "SCRIPT");
      }
      // process tag 
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object)")]
    public static void Main()
    {
      CultureInfo culture = CultureInfo.CurrentCulture;
      Console.WriteLine("The current culture is {0}", culture.Name);
      ProcessTag("script");
      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
      Environment.Exit(0);
    }
  }
}


































































/*
private static void processTag(string tag)
{
    if (tag.ToUpper(CultureInfo.InvariantCulture).Equals("SCRIPT"))
    {
        Console.WriteLine("script".ToUpper(CultureInfo.InvariantCulture) + " equals " + "SCRIPT");
        return;
    }
    // process tag
}
*/
