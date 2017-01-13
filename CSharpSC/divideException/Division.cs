using System;

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

[assembly: CLSCompliant(true)]
namespace SecureCSharp {
    public static class ExceptionExtension
  {
    public static bool LogException(this Exception e)
    {
      Console.Error.WriteLine(@"Exceptions happen: {0}", e.ToString());
      return false;
    }
  }
  public class DivideException {
    public static int Div(int Sum, int Total) {
      int average = Sum / Total;
      // New code may throw UnauthorizedAccessException 
      System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\divresult.txt");
      file.WriteLine(average);
      file.Close();
      return average;
    }

    public static void Main() {
      try {
        Console.WriteLine(Div(200, 5));
        Console.WriteLine(Div(200, 0)); // Divide by zero
      }
      catch (Exception e) when (e.LogException()) { }
      catch (DivideByZeroException) {
        Console.WriteLine("Divide by zero");
      }
      catch (UnauthorizedAccessException) {
        // Recover by requesting new file name from user
      }
      /*
      catch (Exception e) {
        e.LogException();
      }
      */
       
      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
  }
}
