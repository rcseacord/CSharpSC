using System;
using System.Collections;
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

  public class IntegerComparator : IComparer {
    public int Compare(object x, object y) {
      int i = (int)x;
      int j = (int)y;
      return i < j ? -1 : (i == j ? 0 : 1);
      // return i < j ? -1 : (i > j ? 1 : 0);
    }

    public static void Main() {
      int i, j;
      i = 2147483646; j = 2147483647;
      Console.WriteLine("Compare (wrong) less than: " + i.CompareTo(j));
      /*
      Console.WriteLine("Compare (wrong)  equal to: " + cmpWrong.Compare(2147483647, 2147483647));
      Console.WriteLine("Compare (wrong)  greater than: " + cmpWrong.Compare(2147483647, 2147483646));

      Console.WriteLine("Compare less than: " + cmp.Compare(2147483646, 2147483647));
      Console.WriteLine("Compare equal to: " + cmp.Compare(2147483647, 2147483647));
      Console.WriteLine("Compare greater than: " + cmp.Compare(2147483647, 2147483646));
      */

      // Constructors for class Boolean return distinct objects.
     bool? b1 = new bool?(true);
     bool? b2 = new bool?(true);

      if (b1 != b2) { // never equal
        Console.WriteLine("Never equal");
      }

      // The values of autoboxed Boolean variables may be compared using the
      // reference equality operators because Java guarantees that the 
      // Boolean type is fully memoized.
      b1 = true;
      b2 = true;
      if (b1 == b2) {  // always equal
        Console.WriteLine("Always equal");
      }

      b1 = true;
      if (b1 == (bool)b2) { // always equal
        Console.WriteLine("Always equal");
      }
    }
  }
}
