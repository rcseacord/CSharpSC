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
using System.Collections.Generic;
using System.Linq;

[assembly: CLSCompliant(true)]
namespace ArrayEquals {

  public class Box : IEquatable<Box>
  {
    public Box(int h, int l, int w)
    {
      this.Height = h;
      this.Length = l;
      this.Width = w;
    }
    public int Height { get; }
    public int Length { get; }
    public int Width { get; }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as Box);
    }

    public bool Equals(Box p)
    {
      // If parameter is null return false.
      if (p is null) return false;
      // Optimization for a common success case.
      if (Object.ReferenceEquals(this, p)) return true;
      // If run-time types are not equal return false.
      if (this.GetType() != p.GetType()) return false;

      // Return true if the fields match.
      // The base class is not invoked because
      // Object defines Equals as reference equality.
      return (Height == p.Height) && (Length == p.Length) && (Width == p.Width);
    }

    public override int GetHashCode()
    {
      int hCode = Height * Length * Width;
      return hCode.GetHashCode();
    }

    public static bool operator ==(Box lhs, Box rhs)
    {
      // Check for null on left side.
      if (lhs is null)
      {
        if (rhs is null)
        {
          // null == null = true.
          return true;
        }

        // Only the left side is null.
        return false;
      }
      // Equals handles case of null on right side.
      return lhs.Equals(rhs);
    }

    public static bool operator !=(Box lhs, Box rhs)
    {
      return !(lhs == rhs);
    }
  }

  class BoxSameDimensions : EqualityComparer<Box>
  {
    public override bool Equals(Box b1, Box b2)
    {
      if (b1 is null && b2 is null) return true;
      if (b1 is null || b2 is null) return false;
      return b1.Height == b2.Height && b1.Length == b2.Length && b1.Width == b2.Width;
    }

    public override int GetHashCode(Box bx)
    {
      if (bx is null) throw new ArgumentNullException();
      int hCode = bx.Height ^ bx.Length ^ bx.Width;
      hCode++;
      return hCode.GetHashCode();
    }

  }

  class BoxSameVolume : EqualityComparer<Box>
  {
    public override bool Equals(Box b1, Box b2)
    {
      if (b1 is null && b2 is null) return true;
      if (b1 is null || b2 is null) return false;
      return b1.Height * b1.Width * b1.Length == b2.Height * b2.Width * b2.Length;
    }

    public override int GetHashCode(Box bx)
    {
      if (bx is null) throw new ArgumentNullException();
      int hCode = bx.Height * bx.Length * bx.Width;
      return hCode.GetHashCode();
    }
  }

  public static class CompareArrays {
    static bool ArraysEqual<T>(T[] a1, T[] a2)
    {
      if (ReferenceEquals(a1, a2)) return true;
      if (a1 is null || a2 is null) return false;
      if (a1.Length != a2.Length) return false;
      var comparer = EqualityComparer<T>.Default;
      return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
    }

    private static Dictionary<Box, String> _boxes;

    public static void AddBox(EqualityComparer<Box> boxComparer, Box bx, string name)
    {
      try
      {
        _boxes.Add(bx, name);  // throws ArgumentException if an element with the same key already exists 
        Console.WriteLine("Added {0}, Count = {1}, HashCode = {2}",
          name, _boxes.Count.ToString(), boxComparer.GetHashCode(bx));
      }
      catch (ArgumentException)
      {
        Console.WriteLine("A box equal to {0} is already in the collection.", name);
      }
    }

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

      // .NET 2 or higher solution
      Console.WriteLine(ArraysEqual(arr1, arr2)); // true

      // .NET 3.5 or higher solution
      Console.WriteLine(arr1.SequenceEqual(arr2)); // true

      // .NET 4.5 or higher solution
      Console.WriteLine((arr1 as IStructuralEquatable).Equals(arr2, StructuralComparisons.StructuralEqualityComparer));   // True

      EqualityComparer<Box> boxComparer = new BoxSameDimensions();
      _boxes = new Dictionary<Box, string>(boxComparer);

      Console.WriteLine("Boxes equality by dimensions:");
      AddBox(boxComparer, new Box(8, 4, 8), "red");
      AddBox(boxComparer, new Box(8, 6, 8), "green");
      AddBox(boxComparer, new Box(8, 4, 8), "blue");
      AddBox(boxComparer, new Box(8, 8, 8), "yellow");

      Console.WriteLine();
      Console.WriteLine("Boxes equality by volume:");

      boxComparer = new BoxSameVolume();
      _boxes = new Dictionary<Box, string>(boxComparer);
      AddBox(boxComparer, new Box(8, 4, 8), "pink");
      AddBox(boxComparer, new Box(8, 6, 8), "orange");
      AddBox(boxComparer, new Box(4, 8, 8), "purple");
      AddBox(boxComparer, new Box(8, 8, 4), "brown");

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}
