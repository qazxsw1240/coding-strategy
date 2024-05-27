#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class MoveForwardCommand : AbstractCommand
    {
        private static readonly Coordinate _coordinate=new Coordinate(0,1);

        public MoveForwardCommand(string id="1", string name="앞으로 이동", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade, 0)
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
        public override bool Invoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public override bool Revoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveCoordinateStatement(robotDelegate, _coordinate));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveCoordinateStatement(robotDelegate, _coordinate));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}