#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using Robot;
    using Abnormality;
    using Statement;

    public class AddWormAllEnemyCommand : AbstractCommand
    {

        public AddWormAllEnemyCommand(string id="9", string name="전역 웜 추가", int enhancedLevel=1, int grade=2,
        string explanation="모든 캐릭터에게 웜 1만큼 부여합니다.")
        : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new AddWormAllEnemyCommand();
            }
            return new AddWormAllEnemyCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            _commandBuilder.Append(new AddAbnormalityAllEnemyStatement(robotDelegate, new Worm(robotDelegate), 1));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new AddAbnormalityAllEnemyStatement(robotDelegate, new Worm(robotDelegate), 1));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.AppendFirst(new SuperStatement(robotDelegate));
        }
    }
}