using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
