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
using System.Runtime.Serialization;

[assembly: CLSCompliant(true)]
namespace SecureCSharp
{
  // Violates this rule
  [Serializable]
  internal class FirstCustomException : Exception
  {
    internal FirstCustomException() { }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal FirstCustomException(string message)
        : base(message) { }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal FirstCustomException(string message, Exception innerException)
        : base(message, innerException) { }

    protected FirstCustomException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
  }

  // Does not violate this rule because
  // SecondCustomException is public
  [Serializable]
  public class SecondCustomException : Exception
  {
    public SecondCustomException() { }

    public SecondCustomException(string message)
        : base(message) { }

    public SecondCustomException(string message, Exception innerException)
        : base(message, innerException) { }

    protected SecondCustomException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

  }

  // Does not violate this rule because
  // ThirdCustomException it does not derive directly from
  // Exception, SystemException, or ApplicationException
  [Serializable]
  internal class ThirdCustomException : SecondCustomException
  {
    internal ThirdCustomException() { }

    internal ThirdCustomException(string message)
        : base(message) { }

    internal ThirdCustomException(string message, Exception innerException)
        : base(message, innerException) { }

    protected ThirdCustomException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

   static void Main()
    {
      throw new FirstCustomException();
      throw new SecondCustomException();
      throw new ThirdCustomException();
    }
  }
}
