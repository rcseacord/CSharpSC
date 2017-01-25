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
using System.Text;

namespace SecureCSharp
{
  class StringIntern
  {
    static void Main()
    {
      // Constant strings within the same assembly are always interned by the runtime.
      // This means they are stored in the same location in memory. Therefore, 
      // the two strings have reference equality although no assignment takes place.
      string strA = "Hello world!";
      string strB = "Hello world!";
      Console.WriteLine("ReferenceEquals(strA, strB) = {0}",
                       Object.ReferenceEquals(strA, strB)); // true

      // After a new string is assigned to strA, strA and strB
      // are no longer interned and no longer have reference equality.
      strA = "Goodbye world!";
      Console.WriteLine("strA = \"{0}\" strB = \"{1}\"", strA, strB);

      Console.WriteLine("After strA changes, ReferenceEquals(strA, strB) = {0}",
                      Object.ReferenceEquals(strA, strB)); // false

      // A string that is created at runtime cannot be interned.
      StringBuilder sb = new StringBuilder("Hello world!");
      string stringC = sb.ToString();
      // False:
      Console.WriteLine("ReferenceEquals(stringC, strB) = {0}",
                      Object.ReferenceEquals(stringC, strB));

      // The string class overloads the == operator to perform an equality comparison.
      Console.WriteLine("stringC == strB = {0}", stringC == strB); // true

      // Use String.Copy method to avoid interning.
      string strC = String.Copy(strA);

      Console.WriteLine(String.ReferenceEquals(strA, strC));

      //---- Intern method --------------------------
      string s1 = "MyTest";
      string s2 = String.Copy(s1);
      string s3 = String.Intern(s2);
      Console.WriteLine(String.ReferenceEquals(s1, s2)); // Different references.
      Console.WriteLine(String.ReferenceEquals(s1, s3)); // The same reference.

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}
