using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
      string _s = s ?? throw new ArgumentException(
              paramName: "s",
              message: "null");
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
