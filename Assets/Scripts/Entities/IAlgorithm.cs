#nullable enable


namespace CodingStrategy.Entities
{
    using System.Collections.Generic;

    public interface IAlgorithm : IList<ICommand>
    {
        public abstract int Capacity { get; set; }

        public abstract bool CopyTo(IAlgorithm algorithm);

        public abstract ICommand[] AsArray();
    }
}
