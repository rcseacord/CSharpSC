using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CLSCompliant(true)]
namespace SecureCSharp {
  public static class ExceptionExtension {
    public static bool LogException(this Exception exception) {
      Console.Error.WriteLine("Exceptions happen: {0}", exception?.ToString());
      return false;
    }
  }
  class FinalEscape {

    public static void DoOperation(string some_file) {
      // Code to check or set character encoding
      try {
        StreamReader reader = new StreamReader(some_file);
        reader.Dispose();
        try
        {
          string s = reader.ReadLine();
          Console.Write(s);
        }
        /*
        catch (Exception e)
        {
          Console.Error.WriteLine("Exception: {0}", e.ToString());
        }
        */
        finally {
          try {
            Console.Write((char)reader.Read());  // generate exception
          }
          catch (IOException e) {
            Console.Error.WriteLine("IOException: {0}", e.ToString());
          }
        }
      }
      catch (IOException e) {
        e.LogException();
        // Forward to handler
        throw;
      }
    }

    static void Main()  {
      try {
        DoOperation(
          Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + 
          @"\Visual Studio 2015\Projects\CSharpSC\FinalEscape\FinalEscape.txt");
      }
      catch (Exception e) {
        Console.Error.WriteLine("Exception: {0}", e.ToString());
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}

