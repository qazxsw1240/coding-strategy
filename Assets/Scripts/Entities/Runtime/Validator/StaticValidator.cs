using CodingStrategy.Entities.Board;

namespace CodingStrategy.Entities.Runtime.Validator
{
    public class StaticValidator: IExecutionValidator
    {
        public bool IsValid(IBoardDelegate boardDelegate)
        {
            // TODO: 미구현 상태
            return true;
        }
    }
}
