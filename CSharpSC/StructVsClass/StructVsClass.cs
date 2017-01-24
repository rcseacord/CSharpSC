using System;
using System.Diagnostics;

namespace SecureCSharp
{

  // Value type
  struct Point
  {
    public Point(int x, int y) : this()
    {  // constructor
      this.X = x;
      this.Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
  }

  // Reference type
  class Form
  {

    public Form(string text)
    {
      this.Text = text;
    }

    public string Text { get; set; }

  }


  class Program
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object)")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SecureCSharp.Form.set_Text(System.String)")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SecureCSharp.Form.#ctor(System.String)")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ReferenceEquals")]
    static void Main()
    {
      // Copy or original?
      Point point1 = new Point(20, 30);
      Point point2 = point1;
      point2.X = 50;
      Console.WriteLine(point1.X + ", " + point1.Y);       // 20, 30 
      Console.WriteLine(point2.X + ", " + point1.Y);       // 50, 30

      Form form1 = new Form("Black");
      Form form2 = form1;
      form2.Text = "Blue";
      Console.WriteLine(form1.Text);     // Blue 
      Console.WriteLine(form2.Text);     // Blue

      // Do point1 and point1 point to the same object? 
      Console.WriteLine(ReferenceEquals(point1, point1));  // false

      object o1 = new Point(1, 2);
      object o2 = o1;
      object o3 = o1;
      Console.WriteLine(ReferenceEquals(o2, o3));  // false

      //--- Reference Equality -----------------------------------------
      Point p1 = new Point(1, 2);
      // Value types are copied on assignment. p1 and p2 have 
      // the same values but are not the same object.
      Point p2 = p1;
      Console.WriteLine("After assignment: ReferenceEquals(p1, p2) = {0}", Object.ReferenceEquals(p1, p2)); // false

      // Copy or original?
      Test(point1, form1);
      Console.WriteLine(point1.X);
      Console.WriteLine(form1.Text);
      Test(ref point1, ref form1);
      Console.WriteLine(point1.X);
      Console.WriteLine(form1.Text);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SecureCSharp.Form.set_Text(System.String)")]
    private static void Test(Point p, Form f)
    {
      p.X = 100;                       // No effect on point1 since p is a copy
      f.Text = "Purple";               // This will change form1’s text since
                                       // form1 and f point to the same object
      f = null;                        // No effect on form1
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SecureCSharp.Form.set_Text(System.String)")]
    private static void Test(ref Point p, ref Form f)
    {
      p.X = 100;                      // No effect on point1 since p is a copy
      f.Text = "Purple";              // This will change form1’s text since
                                      // form1 and f point to the same object
      f = null;                       // No effect on form1
    }
  }
}
