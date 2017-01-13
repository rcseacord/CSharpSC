﻿using System;

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

namespace SecureCSharp
{
    public class FPinput
    {
        internal static double currentBalance; // User's cash balance
        internal static void DoDeposit(string userInput)
        {
            double val = 0;
            try
            {
                val = Convert.ToDouble(userInput);
            }
            catch (System.FormatException)
            {
                Console.Error.WriteLine("input format error");
                return;
            }

            if (double.IsInfinity(val))
            {
                Console.Error.WriteLine("input is infinite");
                return;
            }

            if (double.IsNaN(val))
            {
                Console.Error.WriteLine("input is NaN");
                return;
            }

            if (val >= double.MaxValue - currentBalance)
            {
                Console.Error.WriteLine("input range error");
                return;
            }

            Console.Error.WriteLine("updating balance");
            currentBalance += val;

        }

        public static void Main(string[] args)
        {
            DoDeposit("-Infinity");
            DoDeposit("Infinity");
            DoDeposit("NaN");
        }

    }
}
