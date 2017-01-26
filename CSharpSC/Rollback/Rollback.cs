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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{

  public class Book
  {
    private readonly string _Title;

    public Book(string title)
    {
            if (title == null)
            {
                throw new ArgumentNullException(paramName: "title", message: "title cannot be a null reference");
            }
            _Title = title;
    }

    public string Title => _Title;
  }

  class Rollback
  {
    public static void SerializeObjectGraph(FileStream fs, IFormatter formatter, Object rootObj)
    {
      // Save the current position of the file.
      Int64 beforeSerialization = fs.Position;
      try
      {
        // Attempt to serialize the object graph to the file.
        formatter.Serialize(fs, rootObj);
      }
      catch
      {  // Catch all CLS and non-CLS exceptions.
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
    } // end public static void SerializeObjectGraph

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    static void Main()
    {
      using (FileStream fs = new FileStream(@"..\..\DataFile.dat", FileMode.Create))
      {
        SerializeObjectGraph(fs, new BinaryFormatter(), new Book("Of Mice and Men"));
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}














































/*
internal static bool RollbackTransaction(FileStream fs, Int64 b4Serialization)
{
  // Catch all CLS and non-CLS exceptions.
  // If ANYTHING goes wrong, reset the file back to a good state.
  fs.Position = b4Serialization;

  // Truncate the file.
  fs.SetLength(fs.Position);

  // NOTE: The preceding code isn't in a finally block because
  // the stream should be reset only when serialization fails.

  return false;
}
public static void SerializeObjectGraph(FileStream fs, IFormatter formatter, Object rootObj)
{
  // Save the current position of the file.
  Int64 beforeSerialization = fs.Position;
  try
  {
    // Attempt to serialize the object graph to the file.
    formatter.Serialize(fs, rootObj);
  }
  catch when (RollbackTransaction(fs, beforeSerialization))
  {
    Console.WriteLine("Never called");
  }
} // end public static void SerializeObjectGraph
*/
