using System;

namespace CodingStrategy.Entities.Runtime
{
    public class ExecutionException : Exception
    {
        public ExecutionException() {}

        public ExecutionException(string message) : base(message) {}
    }
}
