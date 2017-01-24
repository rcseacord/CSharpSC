using System;
using System.IO;

namespace SecureCSharp
{
  class ReadAndWrite
  {
    static string[] lines = new string[10];
    static void WriteFile(string fileName)
    {
      // Write only some strings in an array to a file.
      using (Stream stream = new FileStream(fileName, FileMode.OpenOrCreate))
      {
        using (StreamWriter writer = new StreamWriter(stream))
        {
          foreach (string line in lines)
          {
              writer.WriteLine(line);
          } // end foreach (string line in lines)
        } // end using (StreamWriter file = new StreamWriter(stream))
      } // end using (Stream stream = new FileStream(fileName, FileMode.OpenOrCreate))
    } // end void WriteFile(string fileName)

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    static void ReadFile(string fileName)
    {
      // Read each line of the file into a string array. Each element
      // of the array is one line of the file.
      int n = 0;
      using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
      {
        while ((lines[n] = reader.ReadLine()) != null)
        {
          n++;
        }
      }

      // Display the file contents by using a foreach loop.
      System.Console.WriteLine("Contents of WriteLines2.txt = ");
      foreach (string line in lines)
      {
        // Use a tab to indent each line of the file.
        Console.WriteLine("\t" + line);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    static void Main()
    {
      ReadFile(@"..\..\file.txt");
      WriteFile(@"..\..\output.txt");

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }  // end static void Main()
  } // end class ReadAndWrite
}  // end namespace SecureCSharp





















































/*
Stream stream = null;
try
{
  stream = new FileStream("file.txt", FileMode.OpenOrCreate);
  using (StreamWriter writer = new StreamWriter(stream))
  {
    stream = null;
    // Use the writer object...  
  }
}
finally
{
  stream?.Dispose();
}
*/
