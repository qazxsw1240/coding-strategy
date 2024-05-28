#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class ReinforceCommand : AbstractCommand
    {
        private readonly IList<Coordinate> _coordinates=new List<Coordinate>();

        public ReinforceCommand(string id="20", string name="보강", int enhancedLevel=1, int grade=2,
        string explanation="현재 로봇의 방어력을 1만큼 이번 런타임동안 증가합니다.")
        : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new ReinforceCommand();
            }
            return new ReinforceCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            _commandBuilder.Append(new AddArmorStatement(robotDelegate, Info.EnhancedLevel));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            return;
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            return;
        }
    }
}