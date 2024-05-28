#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using Robot;
    using Abnormality;
    using Statement;

    public class AddStackCommand : AbstractCommand
    {

        public AddStackCommand(string id="8", string name="스택 추가", int enhancedLevel=1, int grade=2,
        string explanation="자신의 캐릭터에게 스택 2만큼 부여합니다.")
        : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new AddStackCommand();
            }
            return new AddStackCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            _commandBuilder.Append(new AddAbnormalitySpecificRobotStatement(robotDelegate, new Stack(robotDelegate), 2));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddAbnormalitySpecificRobotStatement(robotDelegate, new Stack(robotDelegate), 2));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}