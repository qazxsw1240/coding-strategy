#nullable enable


namespace CodingStrategy.Runtime
{
    using System;

    public class ExecutionException : Exception
    {
        public ExecutionException() : base()
        {
        }

        public ExecutionException(string message) : base(message)
        {
        }
    }
}
