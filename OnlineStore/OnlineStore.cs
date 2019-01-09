using System;
using System.IO;

using InputSource = org.xml.sax.InputSource;
using SAXException = org.xml.sax.SAXException;
using SAXParseException = org.xml.sax.SAXParseException;
using XMLReader = org.xml.sax.XMLReader;
using DefaultHandler = org.xml.sax.helpers.DefaultHandler;

internal class OnlineStore
{

  //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
  //ORIGINAL LINE: private static void createXMLStreamBad(final java.io.BufferedOutputStream outStream, final String quantity) throws java.io.IOException
  //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
  private static void createXMLStreamBad(BufferedOutputStream outStream, string quantity)
  {
    string xmlString = "<item>\n<description>Widget</description>\n" + "<price>500</price>\n" + "<quantity>"
        + quantity + "</quantity></item>";
    outStream.write(xmlString.GetBytes(UTF_8));
    outStream.flush();
  }

  public static void Main(string[] args)
  {
    // Unvalidated input
    try
    {
      using (MemoryStream baos = new MemoryStream(), BufferedOutputStream bos = new BufferedOutputStream(baos))
			{
        createXMLStreamBad(bos, "1");
        createXMLStreamBad(bos, "1</quantity><price>1.0</price><quantity>1");
      }
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine("thrown exception: " + ex.ToString());
      Exception[] suppressed = ex.Suppressed;
      foreach (Exception aSuppressed in suppressed)
      {
        Console.Error.WriteLine("suppressed exception: " + aSuppressed.ToString());
      }
    }
  } // end main
} // end class

//-------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2019 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to convert some aspects of the Java String class.
//-------------------------------------------------------------------------------------------
internal static class StringHelper
{
  //----------------------------------------------------------------------------------
  //	This method replaces the Java String.substring method when 'start' is a
  //	method call or calculated value to ensure that 'start' is obtained just once.
  //----------------------------------------------------------------------------------
  public static string SubstringSpecial(this string self, int start, int end)
  {
    return self.Substring(start, end - start);
  }

  //------------------------------------------------------------------------------------
  //	This method is used to replace calls to the 2-arg Java String.startsWith method.
  //------------------------------------------------------------------------------------
  public static bool StartsWith(this string self, string prefix, int toffset)
  {
    return self.IndexOf(prefix, toffset, System.StringComparison.Ordinal) == toffset;
  }

  //------------------------------------------------------------------------------
  //	This method is used to replace most calls to the Java String.split method.
  //------------------------------------------------------------------------------
  public static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
  {
    string[] splitArray = System.Text.RegularExpressions.Regex.Split(self, regexDelimiter);

    if (trimTrailingEmptyStrings)
    {
      if (splitArray.Length > 1)
      {
        for (int i = splitArray.Length; i > 0; i--)
        {
          if (splitArray[i - 1].Length > 0)
          {
            if (i < splitArray.Length)
              System.Array.Resize(ref splitArray, i);

            break;
          }
        }
      }
    }

    return splitArray;
  }

  //-----------------------------------------------------------------------------
  //	These methods are used to replace calls to some Java String constructors.
  //-----------------------------------------------------------------------------
  public static string NewString(sbyte[] bytes)
  {
    return NewString(bytes, 0, bytes.Length);
  }
  public static string NewString(sbyte[] bytes, int index, int count)
  {
    return System.Text.Encoding.UTF8.GetString((byte[])(object)bytes, index, count);
  }
  public static string NewString(sbyte[] bytes, string encoding)
  {
    return NewString(bytes, 0, bytes.Length, encoding);
  }
  public static string NewString(sbyte[] bytes, int index, int count, string encoding)
  {
    return System.Text.Encoding.GetEncoding(encoding).GetString((byte[])(object)bytes, index, count);
  }

  //--------------------------------------------------------------------------------
  //	These methods are used to replace calls to the Java String.getBytes methods.
  //--------------------------------------------------------------------------------
  public static sbyte[] GetBytes(this string self)
  {
    return GetSBytesForEncoding(System.Text.Encoding.UTF8, self);
  }
  public static sbyte[] GetBytes(this string self, System.Text.Encoding encoding)
  {
    return GetSBytesForEncoding(encoding, self);
  }
  public static sbyte[] GetBytes(this string self, string encoding)
  {
    return GetSBytesForEncoding(System.Text.Encoding.GetEncoding(encoding), self);
  }
  private static sbyte[] GetSBytesForEncoding(System.Text.Encoding encoding, string s)
  {
    sbyte[] sbytes = new sbyte[encoding.GetByteCount(s)];
    encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
    return sbytes;
  }
}
