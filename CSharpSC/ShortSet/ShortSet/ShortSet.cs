using System;
using System.Collections.Generic;

// The MIT License (MIT)
// 
// Copyright (c) 2016 Robert C. Seacord
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

[assembly: CLSCompliant(true)]
namespace SecureCSharp {
  public static class ShortSet {

    public static void Main() {

      HashSet<short?> s = new HashSet<short?>();
      for (int i = 0; i < 10; i++) {
        s.Add((short)i);
        s.Remove((short)i); // tries to remove an Integer
      }
      Console.WriteLine(s.Count);
    }
  }
}














/*
namespace EXP04J
{

    public static class ShortSet
    {

        public static void Main()
        {

            HashSet<short?> s = new HashSet<short?>();
            for (int i = 0; i < 10; i++)
            {
                s.Add((short)i);
                // remove a Short
                if (s.Remove((short)i) == false)
                {
                    // EXP00-J. Do not ignore values returned by methods
                    Console.Error.WriteLine("Error removing " + i);
                }
            }
            Console.WriteLine(s.Count);
        }
    }

}
*/
