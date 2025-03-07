#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime;
using CodingStrategy.Entities.Runtime.Command;

using UnityEngine;

namespace CodingStrategy.Entities
{
    /// <summary>
    ///     명령어 구현을 위한 보조 클래스입니다. Invoke, Revoke, Copy 메서드를 구현하여
    ///     명령어를 구현할 수 있습니다.
    /// </summary>
    public abstract class AbstractCommand : ICommand
    {
        protected readonly CommandBuilder _commandBuilder;

        private readonly int _energyPoint;

        protected AbstractCommand(CommandProfile profile, int enhancedLevel, int energyPoint)
        {
            ID = profile.ID;
            Info = new CommandInfoImpl(profile.Name, enhancedLevel, profile.Grade, profile.Description);
            _energyPoint = energyPoint;
            _commandBuilder = new CommandBuilder();
        }

        public string ID { get; }

        public ICommandInfo Info { get; }

        public virtual IList<IStatement> GetCommandStatements(ICommandContext context, IRobotDelegate robotDelegate)
        {
            _commandBuilder.Clear();
            if (robotDelegate.EnergyPoint < _energyPoint)
            {
                Debug.LogFormat("Energy is insufficient. Robot Id:{0}", robotDelegate.ID);
                return _commandBuilder.Build();
            }
            robotDelegate.EnergyPoint -= _energyPoint;
            AddStatementOnLevel1(robotDelegate);
            if (Info.EnhancedLevel >= 2)
            {
                AddStatementOnLevel2(robotDelegate);
            }
            if (Info.EnhancedLevel >= 3)
            {
                AddStatementOnLevel3(robotDelegate);
            }
            return _commandBuilder.Build();
        }

        public abstract ICommand Copy(bool keepStatus = true);

        protected abstract void AddStatementOnLevel1(IRobotDelegate robotDelegate);

        protected abstract void AddStatementOnLevel2(IRobotDelegate robotDelegate);

        protected abstract void AddStatementOnLevel3(IRobotDelegate robotDelegate);
    }
}
