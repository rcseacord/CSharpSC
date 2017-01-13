using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


[assembly: CLSCompliant(true)]
namespace SecureCSharp {

  public class Book
  {
    private readonly string _Title;

    public Book(string title)
    {
      //      _Title = title ?? throw new ArgumentException();
      //  _Title = title ?? throw new ArgumentNullException();
      // _Title = title ?? throw new ArgumentNullException("title cannot be a null reference", "title");
      // _Title = title ?? throw new ArgumentNullException("title", "title cannot be a null reference");
      _Title = title ?? throw new ArgumentNullException(paramName: "title", message: "title cannot be a null reference");
    }

    public string Title
    {
      get { return _Title; }
    }
  }

  sealed class Apple {
    public String Color { get; set; }
    public override Boolean Equals(Object o) {
      try {
        // throw new OutOfMemoryException();

        // Cast o to an Apple might cause the CLR to throw an InvalidCastException exception
        Apple a = (Apple)o;

        // Not equal if differnet color
        if (Color != a.Color) return false;

        // Equal if same color
        return true;
      }
      // Catch InvalidCastException and return false (not equal if different types)
      catch (Exception) { return false; }
    }
    public override int GetHashCode()
    {
      return Color.GetHashCode();
    }
  }

  class Exceptions {

    public static void SerializeObjectGraph(FileStream fs, IFormatter formatter, Object rootObj) {
      // Save the current position of the file.
      Int64 beforeSerialization = fs.Position;
      try {
        // Attempt to serialize the object graph to the file.
        formatter.Serialize(fs, rootObj);
      }
      catch {  // Catch all CLS and non-CLS exceptions.
         // If ANYTHING goes wrong, reset the file back to a good state.
        fs.Position = beforeSerialization;

        // Truncate the file.
        fs.SetLength(fs.Position);

        // NOTE: The preceding code isn't in a finally block because
        // the stream should be reset only when serialization fails.

        // Let the caller(s) know what happened by
        // rethrowing the SAME exception.
        throw;
      }
    }

    private static int GetInt(int[] array, int index) {
      try {
        return array[index];
      }
      catch (System.IndexOutOfRangeException e) {
        throw new System.ArgumentOutOfRangeException(
            "Parameter index is out of range.", e);
      }
    }

   private static bool IsKCForm(string s) {
      bool KCForm = false;
      try {
        KCForm = s.IsNormalized(NormalizationForm.FormKC);
      }
      catch (NullReferenceException) {
        return false;
      }
      return KCForm;
    }

    public static bool IsKCForm2(string s)
    {
      string _s = s ?? throw new ArgumentNullException(
          paramName: "s",
          message: "s cannot be a null reference"
        );
      return _s.IsNormalized(
        NormalizationForm.FormKC
      );
    }


    static void Main() {

      string s1 = new String(new char[] { '\u0063', '\u0301', '\u0327', '\u00BE' });
      if (IsKCForm(s1)) {
        Console.WriteLine("NFKC");
      }
      if (!IsKCForm(null)) Console.WriteLine("Not NFKC");
 
      int[] array = { 1, 2, 3 };
      int index = 5;
      Console.WriteLine(GetInt(array, index));

      String Red = "Red";
      Apple RedApple = new Apple() { Color = "red" };
      Apple GreenApple = new Apple() { Color = "green" };
      if (!RedApple.Equals(GreenApple)) Console.WriteLine("different");
      if (!RedApple.Equals(Red)) Console.WriteLine("different");

    }
  }
}
