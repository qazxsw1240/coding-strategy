using System.Diagnostics.CodeAnalysis;

using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public abstract class AbstractAbnormality : IAbnormality
    {
        protected readonly IRobotDelegate _robotDelegate;
        protected int _value;

        protected AbstractAbnormality(string name, IRobotDelegate robotDelegate, int value)
        {
            Name = name;
            _robotDelegate = robotDelegate;
            PlayerDelegate = null;
            _value = value;
        }

#region IAbnormality Members

        public virtual string Name { get; }

        public IRobotDelegate RobotDelegate => _robotDelegate;

        [MaybeNull]
        public IPlayerDelegate PlayerDelegate { get; set; }

        public abstract int Value { get; set; }

        public abstract void Execute();

        public abstract IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false);

#endregion
    }
}
