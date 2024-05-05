#nullable enable


using System.Linq;
using CodingStrategy.Entities.Robot;

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
                if (cellDelegate.Placeables.Count(p => p is IRobotDelegate) > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
