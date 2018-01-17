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
using System.Collections.Generic;
using System.Diagnostics;

namespace EqualsStruct
{
  internal struct TwoDPoint : IEquatable<TwoDPoint>
  {
    // Read/write auto-implemented properties.
    public int X { get; }
    public int Y { get; }

    public TwoDPoint(int x, int y)
        : this()
    {
      X = x;
      Y = y;
    }

    public override bool Equals(object obj)
    {
      if (obj is TwoDPoint point)
      {
        return this.Equals(point);
      }
      return false;
    }

    public bool Equals(TwoDPoint p)
    {
      return (X == p.X) && (Y == p.Y);
    }

    public override int GetHashCode()
    {
      return ShiftAndWrap(X.GetHashCode(), 2) ^ Y.GetHashCode();
    }

    private static int ShiftAndWrap(int value, int positions)
    {
      positions = positions & 0x1F;

      // Save the existing bit pattern, but interpret it as an unsigned integer.
      uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
      // Preserve the bits to be discarded.
      uint wrapped = number >> (32 - positions);
      // Shift and wrap the discarded bits.
      return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
    }

    public static bool operator ==(TwoDPoint lhs, TwoDPoint rhs)
    {
      // Equals handles case of null on right side.
      return lhs.Equals(rhs);
    }

    public static bool operator !=(TwoDPoint lhs, TwoDPoint rhs)
    {
      return !lhs.Equals(rhs);
    }
  }

  class Struct2DPoint
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    static void Main()
    {
      TwoDPoint pointA = new TwoDPoint(3, 4);
      TwoDPoint pointB = new TwoDPoint(3, 4);

      // Compare using virtual Equals
      Debug.Assert(pointA.Equals(pointB), "FAILED: pointA.Equals(pointB)");
      Debug.Assert(pointA == pointB, "FAILED: pointA == pointB)");
      Debug.Assert(!pointA.Equals(null), "FAILED: pointA.Equals(null)");

      // Compare using static Equals
      Debug.Assert(Object.Equals(pointA, pointB), "FAILED:  Object.Equals(pointA, pointB)");
      Debug.Assert(Object.Equals(pointB, pointA), "FAILED:  Object.Equals(pointB, pointA)");

      // Compare using reference equality and inequality operators
      Debug.Assert(pointA == pointB, "FAILED: pointA == pointB)");
      Debug.Assert(!(pointA != pointB), "FAILED: !(pointA != pointB)");
      Debug.Assert(!(pointA == null), "FAILED: !(pointA == null)");
      Debug.Assert((pointA != null), "FAILED: pointA != null");
      Debug.Assert(!(null == pointA), "FAILED: !(null == pointA)");
      Debug.Assert(null != pointA, "FAILED: pointA != null)");

      Debug.Assert(
        EqualityComparer<TwoDPoint>.Default.Equals(pointA, pointB), 
        "FAILED: EqualityComparer<TwoDPoint>.Default.Equals(pointA, pointB)"
      );

      // Compare unboxed to boxed.
      System.Collections.ArrayList list = new System.Collections.ArrayList
            {
                new TwoDPoint(3, 4)
            };
      Debug.Assert(pointA.Equals(list[0]), "FAILED:  pointA.Equals(list[0])");

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}