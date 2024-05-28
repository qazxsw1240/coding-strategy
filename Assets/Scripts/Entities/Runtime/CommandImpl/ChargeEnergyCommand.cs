#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using System.Linq;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class ChargeEnergyCommand : AbstractCommand
    {
        private readonly IList<Coordinate> _coordinates=new List<Coordinate>();

        public ChargeEnergyCommand(string id="17", string name="에너지 충전", int enhancedLevel=1, int grade=1,
        string explanation="사용시 현재 에너지를 1 충전합니다.")
        : base(id, name, enhancedLevel, grade, 0, explanation)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new ChargeEnergyCommand();
            }
            return new ChargeEnergyCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            _commandBuilder.Append(new AddEnergyStatement(robotDelegate, Info.EnhancedLevel));
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