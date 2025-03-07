#nullable enable

using System.Collections.Generic;

namespace CodingStrategy.Entities
{
    public interface IAlgorithm : IList<ICommand>
    {
        public abstract int Capacity { get; set; }

        public abstract bool CopyTo(IAlgorithm algorithm);

        public abstract ICommand[] AsArray();
    }
}
