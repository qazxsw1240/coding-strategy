namespace CodingStrategy.Entities.Shop
{
    using System.Collections.Generic;
    public interface ICommandList : IList<ICommand>
    {
        public abstract ICommand SelectRandomCommand();
    }
}