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

  public static class CardinalityClass {
   private static int CardinalityBad(string value, ICollection<string> col) {
      int count = 0;

      IEnumerator<string> it = col.GetEnumerator();
      while (it.MoveNext()) {
        string elt = it.Current;
        // Because membership in the collection is checked using
        // the expression obj.equals(elt), a null pointer dereference
        // is guaranteed whenever obj is null and elt is not null.
        if ( (value == null && elt == null) || value.Equals(elt) ) {
          count++;
        }
      }
      return count;
    }

    // returns the number of occurrences of Object obj in Collection col.
    private static int Cardinality(string value, ICollection<string> col) {
      int count = 0;

      IEnumerator<string> it = col.GetEnumerator();
      while (it.MoveNext()) {
        string elt = it.Current;
        if (string.Equals(value, elt)) {
          count++;
        }
      }
      return count;
    }

    public static void Main() {
      IList<string> MyList = new List<string>
      {
        "Java", "C", null, "C++"
      };
      Console.WriteLine("Cardinality of my list: " + Cardinality("C", MyList));
      Console.WriteLine("Cardinality of my list: " + Cardinality(null, MyList));

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }

  }
}
