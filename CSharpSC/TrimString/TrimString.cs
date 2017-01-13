using System;
using System.Collections.Generic;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{
static public class TrimString
  {

    private static string TrimBad(string str)
    {
      char ch;
      int i;
      for (i = 0; i < str.Length; i += 1)
      {
        ch = str[i];
        if (!char.IsLetter(ch))
        {
          break;
        }
      }
      return str.Substring(i);
    }

    private static string TrimGood(string str)
    {
      int i = 0;
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

    public static void Main(string[] args)
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