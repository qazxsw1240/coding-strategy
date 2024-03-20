#nullable enable


namespace CodingStrategy.Runtime
{
    using CodingStrategy.Entities.Board;

    public interface IExecutionValidator
    {
        public bool IsValid(IBoardDelegate boardDelegate);
    }
}
