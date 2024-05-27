#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using CodingStrategy.Entities.Robot;

    public class EmptyCommand : AbstractCommand
    {

        public EmptyCommand(string id="0", string name="빈 커맨드", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade, 0)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new EmptyCommand();
            }
            return new EmptyCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
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
            return;
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