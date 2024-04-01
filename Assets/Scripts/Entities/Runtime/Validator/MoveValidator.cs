#nullable enable


using CodingStrategy.Entities.Board;

namespace CodingStrategy.Runtime.Validator
{
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
