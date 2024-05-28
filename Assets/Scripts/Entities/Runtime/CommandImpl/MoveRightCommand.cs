#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class MoveRightCommand : AbstractCommand
    {
        private static readonly Coordinate _coordinate=new(1,0);

        public MoveRightCommand(string id="3", string name="오른쪽으로 이동", int enhancedLevel=1, int grade=1,
        string explanation="바라보는 기준에서 오른쪽으로 1칸 이동합니다.")
        : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new MoveRightCommand();
            }
            return new MoveRightCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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