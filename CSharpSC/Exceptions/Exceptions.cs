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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{

    public class Book
    {
        private readonly string _Title;

        public Book(string title)
        {
            //      _Title = title ?? throw new ArgumentException();
            //  _Title = title ?? throw new ArgumentNullException();
            // _Title = title ?? throw new ArgumentNullException("title cannot be a null reference", "title");
            // _Title = title ?? throw new ArgumentNullException("title", "title cannot be a null reference");
            if (title == null)
            {
                throw new ArgumentNullException(paramName: "title", message: "title cannot be a null reference");
            }
            _Title = title;
        }

        public string Title => _Title;
    }

    sealed class Apple
    {
        public String Color { get; set; }
        public override Boolean Equals(Object o)
        {
            try
            {
                // throw new OutOfMemoryException();

                // Cast o to an Apple might cause the CLR to throw an InvalidCastException exception
                Apple a = (Apple)o;

                // Not equal if differnet color
                if (Color != a.Color) return false;

                // Equal if same color
                return true;
            }
            // Catch InvalidCastException and return false (not equal if different types)
            catch (Exception) { return false; }
        }
        public override int GetHashCode()
        {
            return Color.GetHashCode();
        }
    }

    class Exceptions
    {

        public static void SerializeObjectGraph(FileStream fs, IFormatter formatter, Object rootObj)
        {
            // Save the current position of the file.
            Int64 beforeSerialization = fs.Position;
            try
            {
                // Attempt to serialize the object graph to the file.
                formatter.Serialize(fs, rootObj);
            }
            catch
            {  // Catch all CLS and non-CLS exceptions.
               // If ANYTHING goes wrong, reset the file back to a good state.
                fs.Position = beforeSerialization;

                // Truncate the file.
                fs.SetLength(fs.Position);

                // NOTE: The preceding code isn't in a finally block because
                // the stream should be reset only when serialization fails.

                // Let the caller(s) know what happened by
                // rethrowing the SAME exception.
                throw;
            }
        }

        private static int GetInt(int[] array, int index)
        {
            try
            {
                return array[index];
            }
            catch (System.IndexOutOfRangeException e)
            {
                throw new System.ArgumentOutOfRangeException(
                    "Parameter index is out of range.", e);
            }
        }

        private static bool IsKCForm(string s)
        {
            bool KCForm = false;
            try
            {
                KCForm = s.IsNormalized(NormalizationForm.FormKC);
            }
            catch (NullReferenceException)
            {
                return false;
            }
            return KCForm;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static bool IsKCForm2(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(
                  paramName: "s",
                  message: "s cannot be a null reference"
                );
            }
            string _s = s;
            return _s.IsNormalized(
              NormalizationForm.FormKC
            );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
        static void Main()
        {

            SerializeObjectGraph(new FileStream(@"..\..\DataFile.dat", FileMode.Create), new BinaryFormatter(), new Book("Of Mice and Men"));

            string s1 = new String(new char[] { '\u0063', '\u0301', '\u0327', '\u00BE' });
            if (IsKCForm(s1))
            {
                Console.WriteLine("NFKC");
            }
            if (!IsKCForm(null)) Console.WriteLine("Not NFKC");

            int[] array = { 1, 2, 3 };
            int index = 5;
            Console.WriteLine(GetInt(array, index));

            String Red = "Red";
            Apple RedApple = new Apple() { Color = "red" };
            Apple GreenApple = new Apple() { Color = "green" };
            if (!RedApple.Equals(GreenApple)) Console.WriteLine("different");
            if (!RedApple.Equals(Red)) Console.WriteLine("different");
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
