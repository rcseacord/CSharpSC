﻿// The MIT License (MIT)
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

namespace SecureCSharp {
  using System;
  class TwoDPoint : IEquatable<TwoDPoint>   {
    // Readonly auto-implemented properties.
    public int X { get; private set; }
    public int Y { get; private set; }

    // Set the properties in the constructor.
    public TwoDPoint(int x, int y) {
      if ((x < 1) || (x > 2000) || (y < 1) || (y > 2000))
        throw new System.ArgumentException("Point must be in range 1 - 2000");
      this.X = x;
      this.Y = y;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as TwoDPoint);
    }

    public bool Equals(TwoDPoint p)
    {
      // If parameter is null, return false.
      if (Object.ReferenceEquals(p, null))
      {
        return false;
      }

      // Optimization for a common success case.
      if (Object.ReferenceEquals(this, p))
      {
        return true;
      }

      // If run-time types are not exactly the same, return false.
      if (this.GetType() != p.GetType())
        return false;

      // Return true if the fields match.
      // Note that the base class is not invoked because it is
      // System.Object, which defines Equals as reference equality.
      return (X == p.X) && (Y == p.Y);
    }

    public override int GetHashCode()
    {
      return X * 0x00010000 + Y;
    }

    public static bool operator ==(TwoDPoint lhs, TwoDPoint rhs)
    {
      // Check for null on left side.
      if (Object.ReferenceEquals(lhs, null))
      {
        if (Object.ReferenceEquals(rhs, null))
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

    public static bool operator !=(TwoDPoint lhs, TwoDPoint rhs)
    {
      return !(lhs == rhs);
    }
  }

  // For the sake of simplicity, assume a ThreeDPoint IS a TwoDPoint.
  class ThreeDPoint : TwoDPoint, IEquatable<ThreeDPoint>
  {
    public int Z { get; private set; }

    public ThreeDPoint(int x, int y, int z)
        : base(x, y)
    {
      if ((z < 1) || (z > 2000))
        throw new System.ArgumentException("Point must be in range 1 - 2000");
      this.Z = z;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as ThreeDPoint);
    }

    public bool Equals(ThreeDPoint p)
    {
      // If parameter is null, return false.
      if (Object.ReferenceEquals(p, null))
      {
        return false;
      }

      // Optimization for a common success case.
      if (Object.ReferenceEquals(this, p))
      {
        return true;
      }

      // Check properties that this class declares.
      if (Z == p.Z)
      {
        // Let base class check its own fields 
        // and do the run-time type comparison.
        return base.Equals((TwoDPoint)p);
      }
      else
        return false;
    }

    public override int GetHashCode()
    {
      return (X * 0x100000) + (Y * 0x1000) + Z;
    }

    public static bool operator ==(ThreeDPoint lhs, ThreeDPoint rhs)
    {
      // Check for null.
      if (Object.ReferenceEquals(lhs, null))
      {
        if (Object.ReferenceEquals(rhs, null))
        {
          // null == null = true.
          return true;
        }

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

    static void Main()
    {

      // --------------------
      TwoDPoint tp1 = new TwoDPoint(3, 4);
      TwoDPoint tp2 = new TwoDPoint(3, 4);
      Console.WriteLine(tp1.Equals(tp2));
      Console.WriteLine(((Object)tp1).Equals(tp2));
      Console.WriteLine((tp1 as Object).Equals(tp2));
      Object tp4 = new TwoDPoint(3, 4);
      Console.WriteLine(tp4.Equals(tp2));
      Console.WriteLine(Object.Equals(tp1, tp2));
      Object tp3 = new TwoDPoint(3, 4);
      Console.WriteLine(tp3.Equals(tp2));

      //---------------------------

      ThreeDPoint pointA = new ThreeDPoint(3, 4, 5);
      ThreeDPoint pointB = new ThreeDPoint(3, 4, 5);
      ThreeDPoint pointC = null;
      int i = 5;

      Console.WriteLine("pointA.Equals(pointB) = {0}", pointA.Equals(pointB));
      Console.WriteLine("pointA == pointB = {0}", pointA == pointB);
      Console.WriteLine("null comparison = {0}", pointA.Equals(pointC));
      Console.WriteLine("Compare to some other type = {0}", pointA.Equals(i));

      TwoDPoint pointD = null;
      TwoDPoint pointE = null;

      Console.WriteLine("Two null TwoDPoints are equal: {0}", pointD == pointE);

      pointE = new TwoDPoint(3, 4);
      Console.WriteLine("(pointE == pointA) = {0}", pointE == pointA);
      Console.WriteLine("(pointA == pointE) = {0}", pointA == pointE);
      Console.WriteLine("(pointA != pointE) = {0}", pointA != pointE);

      System.Collections.ArrayList list = new System.Collections.ArrayList
      {
        new ThreeDPoint(3, 4, 5)
      };
      Console.WriteLine("pointE.Equals(list[0]): {0}", pointE.Equals(list[0]));

      // Keep the console window open in debug mode.
      System.Console.WriteLine("Press any key to exit.");
      System.Console.ReadKey();
    }
  }

  /* Output:
      pointA.Equals(pointB) = True
      pointA == pointB = True
      null comparison = False
      Compare to some other type = False
      Two null TwoDPoints are equal: True
      (pointE == pointA) = False
      (pointA == pointE) = False
      (pointA != pointE) = True
      pointE.Equals(list[0]): False
  */
}