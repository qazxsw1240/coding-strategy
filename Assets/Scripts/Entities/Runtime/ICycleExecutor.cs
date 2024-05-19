#nullable enable


namespace CodingStrategy.Runtime
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Board;
    using CodingStrategy.Entities.Robot;

    public interface ICycleExecutor
    {
        public abstract IBoardDelegate BoardDelegate { get; set; }

        public abstract void AddStatement(IStatement statement);

        public abstract IList<IRobotDelegate> ExecuteStatements();

        public abstract bool CheckBoardValidity(IExecutionValidator validator);
    }
}
