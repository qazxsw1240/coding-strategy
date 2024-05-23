#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using Robot;
    using Abnormality;
    using Statement;

    public class AddStackCommand : AbstractCommand
    {
        private readonly CommandBuilder _commandBuilder=new();

        public AddStackCommand(string id="8", string name="스택 추가", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade)
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

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _commandBuilder.Clear();
            _commandBuilder.Append(new AddAbnormalitySpecificRobotStatement(robot, new Stack(robot), 2));
            if(Info.EnhancedLevel>=2)
                _commandBuilder.Append(new AddAbnormalitySpecificRobotStatement(robot, new Stack(robot), 2));
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