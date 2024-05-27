#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class BotnetsCommand : AbstractCommand
    {
        private static readonly Coordinate[] relativePosition={
            new(1, 0), new(-1, 0),
            new(0, 1), new(0, -1),
            new(1, 1), new(1, -1),
            new(-1, 1), new(-1, -1)
        };

        public BotnetsCommand(string id="12", string name="봇네츠", int enhancedLevel=1, int grade=5)
        : base(id, name, enhancedLevel, grade, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new BotnetsCommand();
            }
            return new BotnetsCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            _commandBuilder.Append(new AttackStatement(robotDelegate, new AttackStrategy(Info.EnhancedLevel), relativePosition));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            return;
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            return;
        }

        private class AttackStrategy : IRobotAttackStrategy
        {
            private static readonly int[] _damageArray= { 0, 1, 2, 4};
            private readonly int _damage;
            public AttackStrategy(int enhancedLevel)
            {

                _damage=_damageArray[enhancedLevel];
            } 
            public int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target)
            {
                return _damage;
            }
        }
    }
}