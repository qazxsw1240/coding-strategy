#nullable enable


using System;

namespace CodingStrategy.Entities.Runtime
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
