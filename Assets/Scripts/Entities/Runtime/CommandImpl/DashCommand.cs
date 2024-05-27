#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class DashCommand : AbstractCommand
    {
        private readonly IList<Coordinate> _coordinates=new List<Coordinate>();

        public DashCommand(string id="19", string name="돌진", int enhancedLevel=1, int grade=2)
        : base(id, name, enhancedLevel, grade, 1)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new DashCommand();
            }
            return new DashCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            _commandBuilder
            .Append(new MoveStatement(robotDelegate, 1))
            .Append(new MoveStatement(robotDelegate, 1))
            .Append(new MoveStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveStatement(robotDelegate, 1));
        }

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Append(new MoveStatement(robotDelegate, 1));
        }
    }
}