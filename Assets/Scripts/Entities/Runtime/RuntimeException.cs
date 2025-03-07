#nullable enable

using System;

namespace CodingStrategy.Entities.Runtime
{
    public class RuntimeException : Exception
    {
        public RuntimeException() {}

        public RuntimeException(string message) : base(message) {}
    }
}
