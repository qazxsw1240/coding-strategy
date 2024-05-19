#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.Robot;
    using Statement;

    public class RotateRightCommand : AbstractCommand
    {
        private readonly CommandBuilder _commandBuilder=new();

        public RotateRightCommand(string id="5", string name="우회전", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new RotateRightCommand();
            }
            return new RotateRightCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _commandBuilder.Clear();
            _commandBuilder.Append(new RotateStatement(robot, 1));
            if(Info.EnhancedLevel>=2)
                _commandBuilder.Append(new RotateStatement(robot, 1));
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
    }
}