using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCSharp {
  class Program  {
    static void Main() {
      int? n = null;

      //int m1 = n;      // Will not compile.
      int m2 = (int)n;   // Compiles, but will create an System.InvalidOperationException if n is null.
      int m3 = n.Value;  // Compiles, but will create an System.InvalidOperationException if n is null.
    }
  }
}
