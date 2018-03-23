// The MIT License (MIT)
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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

[assembly: CLSCompliant(true)]
namespace ExceptionHierarchy
{
  public static class Program
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object,System.Object)")]
    public static void Main()
    {
      // Explicitly load the assemblies that we want to reflect over
      LoadAssemblies();
      // Initialize our counters and our exception type list
      int totalPublicTypes = 0, totalExceptionTypes = 0;
      var exceptionTree = new List<string>();
      // Iterate through all assemblies loaded in this AppDomain
      foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
      {
        // Iterate through all types defined in this assembly
        foreach (Type t in a.GetExportedTypes())
        {
          totalPublicTypes++;
          // Ignore type if not a public class
          if (!t.IsClass || !t.IsPublic) continue;
          // Build a string of the type's derivation hierarchy
          var typeHierarchy = new StringBuilder(t.FullName, 5000);
          // Assume that the type is not an Exception-derived type
          bool derivedFromException = false;
          // See if System.Exception is a base type of this type
          Type baseType = t.BaseType;
          while (baseType != null && !derivedFromException)
          {
            // Append the base type to the end of the string
            typeHierarchy.Append("-" + baseType);
            derivedFromException = baseType == typeof(Exception);
            baseType = baseType.BaseType;
          }
          // No more bases and not Exception-derived, try next type
          if (!derivedFromException) continue;
          // We found an Exception-derived type
          totalExceptionTypes++;
          // For this Exception-derived type,
          // reverse the order of the types in the hierarchy
          string[] h = typeHierarchy.ToString().Split('-');
          Array.Reverse(h);
          // Build a new string with the hierarchy in order
          // from Exception -> Exception-derived type
          // Add the string to the list of Exception types
          exceptionTree.Add(string.Join("-", h, 1, h.Length - 1));
        }
      }
      // Sort the Exception types together in order of their hierarchy
      exceptionTree.Sort();
      // Display the Exception tree
      foreach (string s in exceptionTree)
      {
        // For this Exception type, split its base types apart
        string[] x = s.Split('-');
        // Indent based on the number of base types
        // and then show the most-derived type
        Console.WriteLine(new string(' ', 3 * x.Length) + x[x.Length - 1]);
      }
      // Show final status of the types considered
      Console.WriteLine("\n---> of {0} types, {1} are " +
                        "derived from System.Exception.",
          totalPublicTypes, totalExceptionTypes);

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }
    private static void LoadAssemblies()
    {
      string[] assemblies = {
                "System, PublicKeyToken={0}",
                "System.Data, PublicKeyToken={0}",
                "System.Design, PublicKeyToken={1}",
                "System.DirectoryServices, PublicKeyToken={1}",
                "System.Drawing, PublicKeyToken={1}",
                "System.Drawing.Design, PublicKeyToken={1}",
                "System.Management, PublicKeyToken={1}",
                "System.Messaging, PublicKeyToken={1}",
                "System.Runtime.Remoting, PublicKeyToken={0}",
                "System.Security, PublicKeyToken={1}",
                "System.ServiceProcess, PublicKeyToken={1}",
                "System.Web, PublicKeyToken={1}",
                "System.Web.RegularExpressions, PublicKeyToken={1}",
                "System.Web.Services, PublicKeyToken={1}",
                "System.Windows.Forms, PublicKeyToken={0}",
                "System.Xml, PublicKeyToken={0}"
            };
      const string ecmaPublicKeyToken = "b77a5c561934e089";
      const string msPublicKeyToken = "b03f5f7f11d50a3a";
      // Get the version of the assembly containing System.Object
      // We'll assume the same version for all the other assemblies
      Version version = typeof(object).Assembly.GetName().Version;
      // Explicitly load the assemblies that we want to reflect over
      foreach (string a in assemblies)
      {
        string assemblyIdentity = string.Format(
          CultureInfo.InvariantCulture, a, ecmaPublicKeyToken, msPublicKeyToken) + ", Culture=neutral, Version=" + version;
        Assembly.Load(assemblyIdentity);
      }
    }
  }
}
 