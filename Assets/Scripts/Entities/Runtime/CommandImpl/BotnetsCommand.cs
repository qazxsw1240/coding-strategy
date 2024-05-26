#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System;
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class BotnetsCommand : AbstractCommand
    {
        private readonly CommandBuilder _commandBuilder=new();

        public BotnetsCommand(string id="12", string name="봇네츠", int enhancedLevel=1, int grade=5)
        : base(id, name, enhancedLevel, grade)
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

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _commandBuilder.Clear();
            Coordinate[] relativePosition={
                new Coordinate(1, 0), new Coordinate(-1, 0),
                new Coordinate(0, 1),new Coordinate(0, -1),
                new Coordinate(1, 1),new Coordinate(1, -1),
                new Coordinate(-1, 1),new Coordinate(-1, -1)
            };
            _commandBuilder.Append(new AttackStatement(new BotnetsAttackStrategy(Info.EnhancedLevel), robot, relativePosition));
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
        private class BotnetsAttackStrategy : IRobotAttackStrategy
        {
            private readonly int damage;
            public BotnetsAttackStrategy(int enhancedLevel)
            {
                damage=(int)Math.Pow(2, enhancedLevel-1);
            } 
            public int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target)
            {
                return damage;
            }
        }
    }
}