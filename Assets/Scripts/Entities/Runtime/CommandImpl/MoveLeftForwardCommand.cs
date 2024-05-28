#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class MoveLeftForwardCommand : AbstractCommand
    {
        private static readonly Coordinate _coordinate=new(-1,1);
        public MoveLeftForwardCommand(string id="6", string name="좌측 대각선 이동", int enhancedLevel=1, int grade=2,
        string explanation="바라보는 기준에서 왼쪽 앞 대각선 방향으로 1칸 이동합니다.")
        : base(id, name, enhancedLevel, grade, 0, explanation)
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