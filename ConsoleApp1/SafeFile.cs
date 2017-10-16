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
using System.Security.Permissions;
using System.Xml;

namespace ConsoleApp1
{
    class ImperativeSecurity
    {
        public string Filename { get; set; }

        public void SaveFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return;
            try
            {
                Filename = filename;

                bool isReadOnly =
                    ((File.Exists(Filename)) && ((File.GetAttributes(Filename) & FileAttributes.ReadOnly) != 0));
                if (isReadOnly) File.SetAttributes(Filename, FileAttributes.Normal);

                FileIOPermission fp = new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append,
                    Path.GetFullPath(Filename));
                fp.Demand();
                fp.Assert();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<?xml version='1.0' ?>" +
                            "<book genre='Computer' ISBN='978-0-321-82213-0'>" +
                            "<title>Secure Coding in C and C++</title>" +
                            "</book>");
                doc.Save(Path.GetFullPath(Filename));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exceptions happen: {0}", e.ToString());
            }
        }

        static void Main(string[] args)
        {
            new ImperativeSecurity().SaveFile("fred.xml");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }

}
