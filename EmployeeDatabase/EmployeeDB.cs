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
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace EmployeeDatabase
{

  internal class Employee
  {
  }

  internal class EmployeeDatabase
  {
    private ArrayList _activeEmployees;
    private ArrayList _terminatedEmployees;

    public EmployeeDatabase()
    {
      _activeEmployees = new ArrayList
      {
        new Employee()
      };
      _terminatedEmployees = new ArrayList
      {
        new Employee()
      };
    }

    public void TerminateEmployee(int index)
    {
      // Clone sensitive objects.
      var tempActiveEmployees = (ArrayList)_activeEmployees.Clone();
      var tempTerminatedEmployees = (ArrayList)_terminatedEmployees.Clone();

      // Perform actions on temp objects.
      object employee = tempActiveEmployees[index];
      tempActiveEmployees.RemoveAt(index);
      tempTerminatedEmployees.Add(employee);

      //  RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        Console.WriteLine("In try");
      }
      finally
      {
        Swapper.ListSwap(ref _activeEmployees, ref tempActiveEmployees);
        Swapper.ListSwap(ref _terminatedEmployees, ref tempTerminatedEmployees);
      }

      // Keep the console window open in debug mode.
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();
    }

    public class Swapper
    {
      static Swapper()
      {
        Console.WriteLine("Swapper's static ctor called");
      }

      // [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      public static void ListSwap(ref ArrayList first, ref ArrayList second)
      {
        ArrayList temp = first;
        first = second;
        second = temp;
      }
    }
  }

  class Program
  {
    private static void Main()
    {
      new EmployeeDatabase().TerminateEmployee(0);
    }
  } // end class Program

}  // end namespace EmployeeDatabase