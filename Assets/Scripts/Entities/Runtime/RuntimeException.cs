#nullable enable


using System;

namespace CodingStrategy.Runtime
{
    public class RuntimeException : Exception
    {
        public RuntimeException() : base()
        {
        }

        public RuntimeException(string message) : base(message)
        {
        }
    }
}
