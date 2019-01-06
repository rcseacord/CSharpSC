﻿// The MIT License (MIT)
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

[assembly: CLSCompliant(true)]
namespace FinalEscape
{
  public static class ExceptionExtension {
    public static bool LogException(this Exception exception) {
      Console.Error.WriteLine("Exceptions happen: {0}", exception);
      return false;
    }
  }

  internal static class FinalEscape
  {

    public static void DoOperation(string someFile)
    {
      // Code to check or set character encoding
      try
      {
        var reader = new StreamReader(someFile);
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
        finally
        {
          try
          {
            Console.Write((char) reader.Read()); // generate exception
          }
          catch (IOException e)
          {
            Console.Error.WriteLine("IOException: {0}", e);
          }
        }
      }
      catch (IOException e) when (e.LogException()) { }
    }
  } // end class FinalEscape

  internal class Program { 
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
  public static void Main()  {
      try {
        FinalEscape.DoOperation(
          Environment.GetFolderPath(Environment.SpecialFolder.Personal) + 
          @"\Visual Studio 2015\Projects\CSharpSC\CSharpSC\FinalEscape\FinalEscape.txt");
      }
      catch (Exception e) {
        Console.Error.WriteLine("Exception: {0}", e);
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }  // end Main
  } // end class Program 
} // end namespace FinalEscape

