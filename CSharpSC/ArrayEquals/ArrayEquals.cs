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
using System.Collections;
using System.Linq;

[assembly: CLSCompliant(true)]
namespace ArrayEquals {

  public static class CompareArrays {

    public static void Main() {
      object[] arr1 = { "string", 123, true };
      object[] arr2 = { "string", 123, true };

      // Object.equals() compares array references; not contents.
      Console.WriteLine(arr1.Equals(arr2)); // false

      // The two-argument Arrays.equals() method also 
      // compare the contents of two arrays.
      Console.WriteLine(Array.Equals(arr1, arr2)); // false

      // Use the reference equality operators 
      // to compare references.
      Console.WriteLine(arr1 == arr2); // false

      // .NET 3.5 or higher solution
      Console.WriteLine(arr1.SequenceEqual(arr2)); // true

      // .NET 4.0 or higher solution
      Console.WriteLine((arr1 as IStructuralEquatable).Equals(arr2, StructuralComparisons.StructuralEqualityComparer));   // True

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}
