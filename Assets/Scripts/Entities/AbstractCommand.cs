#nullable enable

using System;
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

        private string _id;

        protected AbstractCommand(CommandProfile profile, int enhancedLevel, int energyPoint)
        {
            _id = profile.ID;
            Info = new CommandInfoImpl(profile.Name, enhancedLevel, profile.Grade, profile.Description);
            _energyPoint = energyPoint;
            _commandBuilder = new CommandBuilder();
        }

        /// <summary>
        ///     기본적인 명령어 정보를 생성합니다.
        /// </summary>
        /// <param name="id">명령어의 ID입니다.</param>
        /// <param name="name">명령어의 이름입니다.</param>
        /// <param name="enhancedLevel">명령어의 강화 단계입니다.</param>
        /// <param name="grade"></param>
        /// <param name="energyPoint">명령어 실행에 필요한 에너지입니다.</param>
        /// <param name="explanation"></param>
        [Obsolete("Use CommandProfile instead", true)]
        protected AbstractCommand(
            string id,
            string name,
            int enhancedLevel,
            int grade,
            int energyPoint,
            string explanation)
        {
            _id = id;
            Info = new CommandInfoImpl(name, enhancedLevel, grade, explanation);
            _energyPoint = energyPoint;
            _commandBuilder = new CommandBuilder();
        }

        public virtual string Id
        {
            get => _id;
            set => _id = value;
        }

        public virtual ICommandInfo Info { get; }

        public ICommandContext? Context { get; set; }

        public virtual IList<IStatement> GetCommandStatements(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Clear();
            if (robotDelegate.EnergyPoint < _energyPoint)
            {
                Debug.LogFormat("Energy is insufficient. Robot Id:{0}", robotDelegate.Id);
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
