#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using Robot;
    using Statement;

    public class RestoringCommand : AbstractCommand
    {

        public RestoringCommand(string id="24", string name="원상 복구", int enhancedLevel=1, int grade=5,
        string explanation="사용시 사용한 로봇의 체력이 4 미만일 경우, 체력을 4로 회복시킵니다. 에너지를 모두 소모합니다.")
        : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new RestoringCommand();
            }
            return new RestoringCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            robotDelegate.EnergyPoint=0;
            _commandBuilder.Append(new AddHealthPointStatement(robotDelegate, 4,4));
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