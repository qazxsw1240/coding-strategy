#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class MoveLeftForwardCommand : AbstractCommand
    {
        private readonly CommandBuilder _commandBuilder=new();
        private readonly Coordinate _coordinate=new Coordinate(-1,1);
        public MoveLeftForwardCommand(string id="6", string name="좌측 대각선 이동", int enhancedLevel=1, int grade=2)
        : base(id, name, enhancedLevel, grade)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new MoveLeftForwardCommand();
            }
            return new MoveLeftForwardCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _commandBuilder.Clear();
            _commandBuilder.Append(new MoveCoordinateStatement(robot, _coordinate));
            if(Info.EnhancedLevel>=2)
                _commandBuilder.Append(new MoveCoordinateStatement(robot, _coordinate));
            if(Info.EnhancedLevel>=3)
                _commandBuilder.Append(new SuperStatement(robot));
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