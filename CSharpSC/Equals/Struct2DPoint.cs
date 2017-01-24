using System;
using System.Diagnostics;

struct TwoDPoint : IEquatable<TwoDPoint>
{
  // Read/write auto-implemented properties.
  public int X { get; private set; }
  public int Y { get; private set; }

  public TwoDPoint(int x, int y)
      : this()
  {
    X = x;
    Y = y;
  }

  public override bool Equals(object obj)
  {
    if (obj is TwoDPoint)
    {
      return this.Equals((TwoDPoint)obj);
    }
    return false;
  }

  public bool Equals(TwoDPoint p)
  {
    return (X == p.X) && (Y == p.Y);
  }

  public override int GetHashCode()
  {
    return X ^ Y;
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
    return !(lhs.Equals(rhs));
  }
}

class Struct2DPoint
{
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
  static void Main()   {
    TwoDPoint pointA = new TwoDPoint(3, 4);
    TwoDPoint pointB = new TwoDPoint(3, 4);
    int i = 5;

    // Compare using virtual Equals, static Equals, and == and != operators.
    // True:
    Debug.Assert(pointA.Equals(pointB), "FAILED:  pointA.Equals(pointB)");
    // True:
    Debug.Assert(pointA == pointB, "FAILED:  pointA == pointB)");
    // True:
    Debug.Assert(Object.Equals(pointA, pointB), "FAILED:  Object.Equals(pointA, pointB)");
    // False:
    Debug.Assert(!pointA.Equals(null), "FAILED:  pointA.Equals(null)");
    // False:
    Debug.Assert(!(pointA == null), "FAILED:  pointA == null)");
    // True:
    Debug.Assert((pointA != null), "FAILED:  pointA != null)");
    // False:
    Debug.Assert(!pointA.Equals(i), "FAILED:  pointA.Equals(i)");
    // CS0019:
    // Console.WriteLine("pointA == i = {0}", pointA == i); 

    // Compare unboxed to boxed.
    System.Collections.ArrayList list = new System.Collections.ArrayList
    {
      new TwoDPoint(3, 4)
    };
    // True:
    Debug.Assert(pointA.Equals(list[0]), "FAILED:  pointA.Equals(list[0])");


    // Compare nullable to nullable and to non-nullable.
    TwoDPoint? pointC = null;
    TwoDPoint? pointD = null;
    // False:
    Debug.Assert(!(pointA == (pointC = null)), "FAILED:  pointA == pointC)");
    // True:
    Debug.Assert(pointC == pointD, "FAILED: pointC == pointD)");

    TwoDPoint temp = new TwoDPoint(3, 4);
    pointC = temp;
    // True:
    Debug.Assert(pointA == pointC, "FAILED:  pointA == (pointC = 3,4)");

    pointD = temp;
    // True:
    Debug.Assert(pointD == pointC, "pointD == (pointC = 3,4)");

    // Keep the console window open in debug mode.
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();
  }
}

        /* Output:
            pointA.Equals(pointB) = True
            pointA == pointB = True
            Object.Equals(pointA, pointB) = True
            pointA.Equals(null) = False
            (pointA == null) = False
            (pointA != null) = True
            pointA.Equals(i) = False
            pointE.Equals(list[0]): True
            pointA == (pointC = null) = False
            pointC == pointD = True
            pointA == (pointC = 3,4) = True
            pointD == (pointC = 3,4) = True
        */
 