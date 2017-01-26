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

namespace SecureCSharp
{
    public static class ExceptionExtension
    {
        public static bool LogException(this Exception exception)
        {
            Console.Error.WriteLine(@"Exceptions happen: {0}", exception?.ToString());
            return false;
        }
    }
    class ExceptionFilter
    {

        static Boolean IsCapitalized(String s)
        {
            if (s == null)
            {
                throw new ArgumentException(
                        paramName: "s",
                        message: "null");
            }
            string _s = s;
            if (String.IsNullOrEmpty(s))
            {
                throw new ArgumentException(
                      paramName: "s",
                      message: "empty");
            }
            if (String.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException(
                      paramName: "s",
                      message: "whitespace");
            }
            return Char.IsUpper(_s, 0);
        }


        static Boolean IsPlaceName(String s)
        {
            try
            {
                return IsCapitalized(s);
            }
            catch (ArgumentException e)
            {
                e.LogException();
            }
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
        static void Main()
        {
            string Colorado = null;
            Console.WriteLine(IsPlaceName(Colorado) ? "is a place" : "is not a place");

            Colorado = "";
            Console.WriteLine(IsPlaceName(Colorado) ? "is a place" : "is not a place");

            Colorado = "   ";
            Console.WriteLine(IsPlaceName(Colorado) ? "is a place" : "is not a place");

            Colorado = "colorado";
            Console.WriteLine(IsPlaceName(Colorado) ? "is a place" : "is not a place");

            Colorado = "Colorado";
            Console.WriteLine(IsPlaceName(Colorado) ? "is a place" : "is not a place");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}





















































































/*
    static Boolean IsPlaceName(String s)
    {
      try
      {
        return IsCapitalized(s);
      }
      catch (Exception e) when (e.LogException()) { }
      catch (ArgumentException e) when (e.Message.Contains("null"))
      {
        Console.Error.WriteLine("Null string: {0}", e.ToString());
      }
      catch (ArgumentException e) when (e.Message.Contains("empty"))
      {
        Console.Error.WriteLine("Empty string: {0}", e.ToString());
      }
      catch (ArgumentException e) when (e.Message.Contains("whitespace"))
      {
        Console.Error.WriteLine("String contains whitespace: {0}", e.ToString());
      }
      catch (ArgumentException e) 
      {
        Console.Error.WriteLine("Unexpected argument exception: {0}", e.ToString());
      }
      return false;
    }
    */
