#nullable enable


namespace CodingStrategy.Entities
{
    using System.Collections.Generic;

    public interface IAlgorithm : IList<ICommand>
    {
        public abstract bool CopyTo(IAlgorithm algorithm);
    }
}
