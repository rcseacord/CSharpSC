using System;
using System.Globalization;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{
  static public class CultureClub
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
    private static void ProcessTag(string tag) {
            if (tag.ToUpper().Equals("SCRIPT")) {
                Console.WriteLine("script".ToUpper() + " equals " + "SCRIPT");
                return;
            }
            // process tag
        }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object)")]
    public static void Main()
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            Console.WriteLine("The current culture is {0}", culture.Name);
            ProcessTag("script");
            Environment.Exit(0);
        }
    }
}


































































/*
private static void processTag(string tag)
{
    if (tag.ToUpper(CultureInfo.InvariantCulture).Equals("SCRIPT"))
    {
        Console.WriteLine("script".ToUpper(CultureInfo.InvariantCulture) + " equals " + "SCRIPT");
        return;
    }
    // process tag
}
*/