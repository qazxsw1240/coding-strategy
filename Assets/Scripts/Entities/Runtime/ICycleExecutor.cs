#nullable enable


namespace CodingStrategy.Entities.Runtime
{
    using System.Collections.Generic;
    using Board;
    using Robot;

    public interface ICycleExecutor
    {
        public abstract IBoardDelegate BoardDelegate { get; set; }

        public abstract void AddStatement(IStatement statement);

        public abstract IList<IRobotDelegate> ExecuteStatements();

        public abstract bool CheckBoardValidity(IExecutionValidator validator);
    }
}
