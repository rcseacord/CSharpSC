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
using System.IO;

namespace MultiDispose
{
  internal static class ReadAndWrite
  {
    private static readonly string[] Lines = new string[10];

    public static void WriteFile(string fileName)
    {
      // Write only some strings in an array to a file.
      using (Stream stream = new FileStream(fileName, FileMode.OpenOrCreate))
      {
        using (var writer = new StreamWriter(stream))
        {
          foreach (string line in Lines)
          {
            writer.WriteLine(line);
          } // end foreach (string line in lines)
        } // end using (StreamWriter file = new StreamWriter(stream))

      } // end using (Stream stream = new FileStream(fileName, FileMode.OpenOrCreate))
    } // end void WriteFile(string fileName)

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    public static void ReadFile(string fileName)
    {
      // Read each line of the file into a string array. Each element
      // of the array is one line of the file.
      int n = 0;
      using (var reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
      {
        while ((Lines[n] = reader.ReadLine()) != null)
        {
          n++;
        }
      }

      // Display the file contents by using a foreach loop.
      Console.WriteLine("Contents of WriteLines2.txt = ");
      foreach (string line in Lines)
      {
        // Use a tab to indent each line of the file.
        Console.WriteLine(line);
      }
    }

  } // end class ReadAndWrite

  internal class MultiDispose
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    private static void Main()
    {
      ReadAndWrite.ReadFile(@"..\..\file.txt");
      ReadAndWrite.WriteFile(@"..\..\output.txt");

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }  // end static void Main()
  } // end class MultiDispose
}  // end namespace MultiDispose





















































/*

  internal static class ReadAndWrite
  {
    private static readonly string[] Lines = new string[10];

    public static void WriteFile(string fileName)
    {
      Stream stream = null;
      try
      {
        stream = new FileStream(fileName, FileMode.OpenOrCreate);
        using (var writer = new StreamWriter(stream))
        {
          stream = null;
          foreach (string line in Lines)
          {
            writer.WriteLine(line);
          } // end foreach (string line in lines)
        }
      }
      finally
      {
        stream?.Dispose();
      }
    } // end void WriteFile(string fileName)

    public static void ReadFile(string fileName)
    {
      // Read each line of the file into a string array. Each element
      // of the array is one line of the file.
      int n = 0;
      Stream stream = null;
      try
      {
        stream = new FileStream(fileName, FileMode.OpenOrCreate);
        using (var reader = new StreamReader(stream))
        {
          stream = null;
          while ((Lines[n] = reader.ReadLine()) != null)
          {
            n++;
          }
        }
      }
      finally
      {
        stream?.Dispose();
      }

      // Display the file contents by using a foreach loop.
      Console.WriteLine("Contents of WriteLines2.txt = ");
      foreach (string line in Lines)
      {
        // Use a tab to indent each line of the file.
        Console.WriteLine(line);
      }
    }
  }  // end class ReadWrite

*/
