// The MIT License (MIT)
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
