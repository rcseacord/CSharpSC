﻿using System;
using System.Text.RegularExpressions;

// The MIT License (MIT)
// 
// Copyright (c) 2016 Robert C. Seacord
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

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{

    public class ValidateString {

        public static string NormalizeandValidate(string s) {


            // Normalize to default to normalization form C 
            //           s = s.Normalize(NormalizationForm.FormKC);
            s = s.Normalize();

            // Validate by checking for angle brackets
            Regex rgx = new Regex("[<>]", RegexOptions.IgnoreCase);
            Match match = rgx.Match(s);
            if (match.Success) {
                // Found black listed tag
                throw new InvalidOperationException();
            }
            else {
                Console.WriteLine("input valid");
            }

            return s;
        }

        public static void Main(string[] args) {
            // Assume input is user controlled
            // \uFE64 is normalized to < and \uFE65 is normalized to > using the
            // NFKC normalization form
            string input = "\uFE64" + "script" + "\uFE65";
            Console.WriteLine("unnormalized string: " + input);
            input = NormalizeandValidate(input);
            Console.WriteLine("normalized string: " + input);
        }
    }
}
