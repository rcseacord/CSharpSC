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
      _Title = title ?? throw new ArgumentNullException(paramName: "title", message: "title cannot be a null reference");
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
