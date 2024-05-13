using System.Collections.Generic;
using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Validator
{
    public class StaticValidator : IExecutionValidator
    {
        public bool IsValid(IBoardDelegate boardDelegate)
        {
            // TODO: 미구현 상태
            return true;
        }

        public IList<IRobotDelegate> GetInvalidRobots(IBoardDelegate boardDelegate)
        {
            return new List<IRobotDelegate>();
        }
    }
}
