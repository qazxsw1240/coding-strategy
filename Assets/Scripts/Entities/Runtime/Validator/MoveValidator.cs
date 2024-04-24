#nullable enable


namespace CodingStrategy.Entities.Runtime.Validator
{
    using Board;

    public class MoveValidator : IExecutionValidator
    {
        public bool IsValid(IBoardDelegate boardDelegate)
        {
            ICellDelegate[,] cellDelegates = boardDelegate.AsArray();
            foreach (ICellDelegate cellDelegate in cellDelegates)
            {
                if (cellDelegate.Robot.Count > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
