// Build with the following commands:
// Uncomment out line 17
// C:\Windows\Microsoft.NET\Framework64\v4.0.30319\ilasm.exe /dll ThrowNonClsCompliantException.il
// C:\Users\rseacord\Documents\Visual Studio 2015\Projects\CSharpSC\NonCLSExceptions>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc /r:ThrowNonClsCompliantException.dll CatchNonClsCompliantException.cs

// CatchNonClsCompliantException.cs
using System;
using System.Runtime.CompilerServices;

[assembly: RuntimeCompatibilityAttribute(WrapNonExceptionThrows = false)]
namespace SecureCSharp
{

  struct HandlesExceptions
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    static void CatchAllExceptions()
    {
      try
      {
        //        ThrowsExceptions.ThrowNonClsException();
      }
      catch (Exception e)
      {
        RuntimeWrappedException rwe = e as RuntimeWrappedException;
        Object o = rwe.WrappedException as Object;
        if (o != null)
        {
          Console.WriteLine(o.ToString());
        }
        else
        {
          // Handle other System.Exception types.  
        }

        // Remove some permission.
        Console.Error.WriteLine("CLS compliant exception: {0}", e.ToString());
      }
      catch
      {
        // Remove the same permission as above.
        Console.WriteLine("Non-CLS compliant exception caught.");
      }
    }

    static void Main()
    {
      HandlesExceptions.CatchAllExceptions();
    }
  }
}
