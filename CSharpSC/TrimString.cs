using System;

public class TrimString
{

    public static string trim(string @string)
    {
        char ch;
        int i;
        for (i = 0; i < @string.Length; i += 1)
        {
            ch = @string[i];
            if (!char.IsLetter(ch))
            {
                break;
            }
        }
        return @string.Substring(i);
    }

    public static void Main(string[] args)
    {
        string s1 = trim("AAß東𐐀001");
        Console.WriteLine(s1);
        string s2 = trim("𐐀𐐀𐐀𐐀𐐀𐐀𐐀𐐀1");
        Console.WriteLine(s2);
    }
}
