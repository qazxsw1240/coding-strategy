#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class MoveForwardCommand : AbstractCommand
    {
        private readonly CommandBuilder _commandBuilder=new();
        private readonly Coordinate _coordinate=new Coordinate(0,1);

        public MoveForwardCommand(string id="1", string name="앞으로 이동", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new MoveForwardCommand();
            }
            return new MoveForwardCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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