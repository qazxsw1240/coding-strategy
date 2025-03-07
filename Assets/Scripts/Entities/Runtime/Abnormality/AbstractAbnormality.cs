using System.Diagnostics.CodeAnalysis;

using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public abstract class AbstractAbnormality : IAbnormality
    {
        protected readonly AbnormalityProfile _profile;
        protected readonly IRobotDelegate _robotDelegate;
        protected int _value;

        protected AbstractAbnormality(AbnormalityProfile profile, IRobotDelegate robotDelegate, int value)
        {
            _robotDelegate = robotDelegate;
            PlayerDelegate = null;
            _value = value;
            _profile = profile;
        }

        public virtual string Name => _profile.Name;

        public IRobotDelegate RobotDelegate => _robotDelegate;

        [MaybeNull]
        public IPlayerDelegate PlayerDelegate { get; set; }

        public abstract int Value { get; set; }

        public abstract void Execute();

        public abstract IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false);
    }
}
