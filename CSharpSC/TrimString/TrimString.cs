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

[assembly: CLSCompliant(true)]
namespace CSharpSC
{
  public static class TrimString
  {

    private static string TrimBad(string str)
    {
      int i;
      for (i = 0; i < str.Length; i += 1)
      {
          var ch = str[i];
          if (!char.IsLetter(ch))
        {
          break;
        }
      }
      return str.Substring(i);
    }

    private static string TrimGood(string str)
    {
      int i;
      for (i = 0; i < str.Length; i += char.IsSurrogatePair(str, i) ? 2 : 1)
      {
        // determine if code point is character
        if (!char.IsLetter(str, i))
        {
          break;
        }
      }
      // return the substring from 0 to the index offset by i code points
      return str.Substring(i);
    }

    public static void Main()
    {
      string s1 = TrimBad("Aß東𐐀001");
      Console.WriteLine(s1);
      string s2 = TrimBad("𐐀𐐀𐐀𐐀𐐀𐐀𐐀𐐀1");
      Console.WriteLine(s2);

      s1 = TrimGood("Aß東𐐀001");
      Console.WriteLine(s1);
      s2 = TrimGood("𐐀𐐀𐐀𐐀𐐀𐐀𐐀𐐀1");
      Console.WriteLine(s2);
    }
  }
}