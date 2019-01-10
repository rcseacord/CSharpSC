// The MIT License (MIT)
// 
// Copyright (c) 2019 Robert C. Seacord
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Execute
{
  class Program
  {
    public static void Main(string[] args)
    {
      const string printArgsCmd = "C:\\Users\\rseacord\\source\\repos\\PrintArgs\\Release\\PrintArgs.exe";

      Console.WriteLine("PrintArgs Path with Spaces");
      Console.WriteLine("PrintArgs.exe argument1 \"argument 2\"  \"\\some\\path with\\spaces\"");
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey();

      try
      {
        using (Process process = new Process())
        {
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.FileName = printArgsCmd;
          process.StartInfo.Arguments = "argument1 \"argument 2\"  \"\\some\\path with\\spaces\"";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();
          //* Read the output (or the error)
          string output = process.StandardOutput.ReadToEnd();
          Console.WriteLine(output);
          string err = process.StandardError.ReadToEnd();
          Console.WriteLine(err);
          process.WaitForExit();
        }
      }
      catch (InvalidOperationException e)
      {
        Console.WriteLine(e.Message);
      }

      Console.WriteLine("+-----------------------------------------------------+");

      Console.WriteLine("PrintArgs Nested Quotes");
      Console.WriteLine("PrintArgs.exe argument1 \"she said, \"you had me at hello\"\"  \"\\some\\path with\\spaces\"");
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey();

      try
      {
        using (Process process = new Process())
        {
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.FileName = printArgsCmd;
          process.StartInfo.Arguments = "argument1 \"she said, \"you had me at hello\"\"  \"\\some\\path with\\spaces\"";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();
          // Read the output (or the error)
          string output = process.StandardOutput.ReadToEnd();
          Console.WriteLine(output);
          string err = process.StandardError.ReadToEnd();
          Console.WriteLine(err);
          process.WaitForExit();
        }
      }
      catch (InvalidOperationException e)
      {
        Console.WriteLine(e.Message);
      }

      Console.WriteLine("+-----------------------------------------------------+");

      Console.WriteLine("PrintArgs Unbalanced Quotes");
      Console.WriteLine("PrintArgs argument1 \"argument\"2\" argument3 argument4");
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey();

      try
      {
        using (Process process = new Process())
        {
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.FileName = printArgsCmd;
          process.StartInfo.Arguments = "argument1 \"argument\"2\" argument3 argument4";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();
          // Read the output (or the error)
          string output = process.StandardOutput.ReadToEnd();
          Console.WriteLine(output);
          string err = process.StandardError.ReadToEnd();
          Console.WriteLine(err);
          process.WaitForExit();
        }
      }
      catch (InvalidOperationException e)
      {
        Console.WriteLine(e.Message);
      }

      Console.WriteLine("+-----------------------------------------------------+");

      Console.WriteLine("PrintArgs trailing backslash");
      Console.WriteLine("PrintArgs \"\\some\\directory with\\spaces\\\" argument2");
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey();

      try
      {
        using (Process process = new Process())
        {
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.FileName = printArgsCmd;
          process.StartInfo.Arguments = "\"\\some\\directory with\\spaces\\\" argument2";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();
          //* Read the output (or the error)
          string output = process.StandardOutput.ReadToEnd();
          Console.WriteLine(output);
          string err = process.StandardError.ReadToEnd();
          Console.WriteLine(err);
          process.WaitForExit();
        }
      }
      catch (InvalidOperationException e)
      {
        Console.WriteLine(e.Message);
      }

      Console.WriteLine("+-----------------------------------------------------+");
      Console.WriteLine("command shell with &calc argument");
      Console.WriteLine("cmd.exe /C dir &calc");
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey();

      try
      {
        using (Process process = new Process())
        {
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.FileName = "cmd.exe";
          process.StartInfo.Arguments = "/C dir &calc";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();
          //* Read the output (or the error)
          string output = process.StandardOutput.ReadToEnd();
          Console.WriteLine(output);
          string err = process.StandardError.ReadToEnd();
          Console.WriteLine(err);
          process.WaitForExit();
        }
      }
      catch (InvalidOperationException e)
      {
        Console.WriteLine(e.Message);
      }

      Console.WriteLine("+-----------------------------------------------------+");

      Console.WriteLine("Avoid invoking an external command by using library methods");
      Console.WriteLine("cmd.exe /C dir &calc");
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey();

      try
      {
        string dir = "&calc";

        List<string> files = new List<string>(Directory.EnumerateFiles(dir));

        foreach (string file in files)
        {
          Console.WriteLine("{0}", file.Substring(file.LastIndexOf("\\") + 1));
        }
      }
      catch (System.IO.DirectoryNotFoundException DNFEx)
      {
        Console.WriteLine(DNFEx.Message);
      }
      catch (UnauthorizedAccessException UAEx)
      {
        Console.WriteLine(UAEx.Message);
      }
      catch (PathTooLongException PathEx)
      {
        Console.WriteLine(PathEx.Message);
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
      Environment.Exit(0);

    }

  }
}
