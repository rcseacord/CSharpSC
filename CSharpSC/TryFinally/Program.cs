using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCSharp {

   class TryFinally {
    private static bool DoLogic()
    {
      try
      {
        throw new System.InvalidOperationException();
 
      }
      finally
      {
       label: Console.WriteLine("logic done");
        Console.WriteLine("logic done");
        goto label;
  
      }
    }
  
  }


  class Program
  {
    static void Main(string[] args)
    {
    }
  }
}
