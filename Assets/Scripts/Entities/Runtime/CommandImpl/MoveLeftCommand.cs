#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class MoveLeftCommand : AbstractCommand
    {
        private readonly CommandBuilder _commandBuilder=new();
        private readonly Coordinate _coordinate=new Coordinate(-1,0);

        public MoveLeftCommand(string id="2", string name="왼쪽으로 이동", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new MoveLeftCommand();
            }
            return new MoveLeftCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _commandBuilder.Clear();
            _commandBuilder.Append(new MoveCoordinateStatement(robot, _coordinate));
            if(Info.EnhancedLevel>=2)
                _commandBuilder.Append(new MoveCoordinateStatement(robot, _coordinate));
            return _commandBuilder.Build();
        }

        public override bool Invoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public override bool Revoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}