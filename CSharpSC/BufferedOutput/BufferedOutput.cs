using System;
using System.IO;

namespace SecureCSharp
{
  class BufferedOutput
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    static void Main()
    {
      // Create a string array with the lines of text
      string[] lines = { "First line", "Second line", "Third line" };

      // Write the string array to a new file named "WriteLines.txt".
      StreamWriter outputFile = new StreamWriter(@"..\..\WriteLines.txt");

      foreach (string line in lines)
      {
        outputFile.WriteLine(line);
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}





































/*
using (StreamWriter outputFile = new StreamWriter(@"..\..\WriteLines.txt"))
{
  foreach (string line in lines)
  {
    outputFile.WriteLine(line);
  }
}
*/
