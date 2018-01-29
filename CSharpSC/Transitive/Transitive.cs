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

namespace Transitive
{
  internal class TwoDPoint : IEquatable<TwoDPoint>
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

    public bool Equals(TwoDPoint p)
    {
      // If parameter is null, return false.
      if (p is null) return false;

      // Optimization for a common success case.
      if (ReferenceEquals(this, p)) return true;

      // If run-time types are not exactly the same, return false.
      // if (GetType() != p.GetType()) return false;

      // Return true if the fields match.
      // Note that the base class is not invoked because it is
      // System.Object, which defines Equals as reference equality.
      return X == p.X && Y == p.Y;
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as TwoDPoint);
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
  internal sealed class ThreeDPoint : TwoDPoint, IEquatable<ThreeDPoint>
  {
    public ThreeDPoint(int x, int y, int z) : base(x, y)
    {
      if (z < 1 || z > 2000)
        throw new ArgumentException("Point must be in range 1 - 2000");
      Z = z;
    }

    public int Z { get; }

    public bool Equals(ThreeDPoint p)
    {
      // If parameter is null, return false.
      if (p is null) return false;

      // Optimization for a common success case.
      if (ReferenceEquals(this, p)) return true;

      // Check properties that this class declares.
      return Z == p.Z && base.Equals(p);
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as ThreeDPoint);
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

    private static void Main()
    {
      ThreeDPoint p1 = new ThreeDPoint(3, 4, 5);
      TwoDPoint p2 = new TwoDPoint(3, 4);
      ThreeDPoint p3 = new ThreeDPoint(3, 4, 7);

      // Breaks transitivity
      Console.WriteLine("p1.Equals(p2) = {0}", p1.Equals(p2));
      Console.WriteLine("p2.Equals(p3) = {0}", p2.Equals(p3));
      Console.WriteLine("p1.Equals(p3) = {0}", p1.Equals(p3));

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  } // class ThreeDPoint
} // namespace