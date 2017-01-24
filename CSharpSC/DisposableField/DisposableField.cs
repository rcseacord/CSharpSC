using System;
using System.IO;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{
  // Implement the IDisposable interface and dispose of unmanaged resources. 
  public class FileStreamHolder
  {
    FileStream newFile;

    public FileStreamHolder()
    {
      newFile = new FileStream(@"..\..\file.txt", FileMode.Open);
    }
    public bool IsInvalid => newFile == null;
    static void Main()
    {
      FileStreamHolder fsh = new FileStreamHolder();
      if (fsh.IsInvalid)
      {
        Console.WriteLine("Invalid FileStream");
      }
      else
      {
        Console.WriteLine(fsh.ToString());
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}






















































/*
public class FileStreamHolder : IDisposable
{
FileStream newFile;
public FileStreamHolder()
{
  newFile = new FileStream(@"..\..\file.txt", FileMode.Open);
}
public bool IsInvalid => newFile == null;
static void Main()
{
  FileStreamHolder fsh = new FileStreamHolder();
  try
  {
    if (fsh.IsInvalid)
    {
      Console.WriteLine("Invalid file stream");
    }
    else
    {
      Console.WriteLine(fsh.ToString());
    }
  }
  finally
  {
    fsh.Dispose();
  }
  // Keep the console window open in debug mode.
  Console.WriteLine("Press any key to exit.");
  Console.ReadKey();
}

#region IDisposable Support
private bool disposedValue = false; // To detect redundant calls

protected virtual void Dispose(bool disposing)
{
  if (!disposedValue)
  {
    if (disposing)
    {
      newFile.Close();
    }
    disposedValue = true;
  }
}

// Because the class does not directly own any unmanaged resources, 
// it does NOT implement a finalizer.

// This code added to correctly implement the disposable pattern.
public void Dispose()
{
  Dispose(true);
  GC.SuppressFinalize(this);
}
#endregion
}
*/
