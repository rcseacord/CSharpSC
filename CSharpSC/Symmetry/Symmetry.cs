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

namespace Symmetry
{
  internal class TwoDPoint
  {
    // Set the properties in the constructor.
    public TwoDPoint(int x, int y)
    {
      if (x < 1 || x > 2000 || y < 1 || y > 2000)
        throw new ArgumentException("Point must be in range 1 - 2000");
      X = x;
      Y = y;
    }

    // Readonly auto-implemented properties.
    public int X { get; }

    public int Y { get; }

    public override bool Equals(object obj)
    {
      // If parameter is null, return false.
      if (obj is null) return false;

      // Optimization for a common success case.
      if (ReferenceEquals(this, obj)) return true;

      // Incompatible types
      if (!(obj is TwoDPoint)) return false;

      // Compare fields
      var p2D = (TwoDPoint)obj;
      return X == p2D.X && Y == p2D.Y;
    }

    public override int GetHashCode()
    {
      return X * 0x00010000 + Y;
    }

    public static bool operator ==(TwoDPoint lhs, TwoDPoint rhs)
    {
      // Check for null on left side.
      if (lhs is null)
      {
        if (ReferenceEquals(rhs, null))
          return true;

        // Only the left side is null.
        return false;
      }
      // Equals handles case of null on right side.
      return lhs.Equals(rhs);
    }

    public static bool operator !=(TwoDPoint lhs, TwoDPoint rhs)
    {
      return !(lhs == rhs);
    }
  }

  // For the sake of simplicity, assume a ThreeDPoint IS a TwoDPoint.
  internal sealed class ThreeDPoint : TwoDPoint
  {
    public ThreeDPoint(int x, int y, int z) : base(x, y)
    {
      if (z < 1 || z > 2000)
        throw new ArgumentException("Point must be in range 1 - 2000");
      Z = z;
    }

    public int Z { get; }

    public override bool Equals(object obj)
    {
      // If parameter is null, return false.
      if (obj is null) return false;

      // Optimization for a common success case.
      if (ReferenceEquals(this, obj)) return true;

      // Incompatible types
      if (!(obj is ThreeDPoint)) return false;

      // Compare fields
      var p3D = (ThreeDPoint)obj;
      return Z == p3D.Z && base.Equals(p3D);
    }

    public override int GetHashCode()
    {
      return X * 0x100000 + Y * 0x1000 + Z;
    }

    public static bool operator ==(ThreeDPoint lhs, ThreeDPoint rhs)
    {
      // Check for null.
      if (lhs is null)
      {
        if (rhs is null)
          return true;

        // Only the left side is null.
        return false;
      }
      // Equals handles the case of null on right side.
      return lhs.Equals(rhs);
    }

    public static bool operator !=(ThreeDPoint lhs, ThreeDPoint rhs)
    {
      return !(lhs == rhs);
    }

  } // end class ThreeDPoint

  public class Person : IEquatable<Person>
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public bool Equals(Person other)
    {
      // If parameter is null, return false.
      if (other is null) return false;

      // Optimization for a common success case.
      if (ReferenceEquals(this, other)) return true;

      // Guard against 'other' being a type derived from Person:
      if (other.GetType() != typeof(Person)) return false;

      // Compare fields
      return this.FirstName == other?.FirstName && this.LastName == other?.LastName;
    }
  } // end class Person

  public class Student : Person, IEquatable<Student>
  {
    public int Level { get; set; }

    public bool Equals(Student other)
    {
      // If parameter is null, return false.
      if (other is null) return false;

      // Optimization for a common success case.
      if (ReferenceEquals(this, other)) return true;

      // Guard against 'other' being a type derived from Student
      if (other.GetType() != typeof(Student)) return false;

      // Compare fields
      return base.Equals(other) && this.Level == other?.Level;
    }
  } // end class Student

  internal class Program
  {
    private static void Main()
    {
      // Break symmetry in virtual Equals method
      var p1 = new TwoDPoint(3, 4);
      var p2 = new ThreeDPoint(3, 4, 5);

      Console.WriteLine("p1.Equals(p2) = {0}", p1.Equals(p2));  // true
      Console.WriteLine("p2.Equals(p1) = {0}", p2.Equals(p1));  // false

      // Break symmetry in type-specific Equals method
      Person p = new Person
      {
        FirstName = "Bill",
        LastName = "Wagner"
      };
      Student s = new Student
      {
        FirstName = "Bill",
        LastName = "Wagner",
        Level = 9000
      };

      // Call resolves Person.Equals(Person)
      Console.WriteLine(p.Equals(s));  // false

      // Call also resolves Person.Equals(Person) due to overload resolution 
      Console.WriteLine(s.Equals(p));  // true

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  } // class ThreeDPoint
} // namespace