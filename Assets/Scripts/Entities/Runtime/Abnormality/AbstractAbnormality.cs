#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using CodingStrategy.Entities.Player;
    using Robot;
    public abstract class AbstractAbnormality : IAbnormality
    {
        private readonly string _name;
        protected readonly IRobotDelegate _robotDelegate;
        protected int _value;
        protected AbstractAbnormality(string name, IRobotDelegate robotDelegate, int value)
        {
            _name = name;
            _robotDelegate = robotDelegate;
            PlayerDelegate=null;
            _value = value;
        }
        public virtual string Name => _name;

        public IRobotDelegate RobotDelegate => _robotDelegate;
        public IPlayerDelegate? PlayerDelegate
        {
            get;
            set;
        }

        public abstract int Value { get; set; }
        public abstract void Execute();
        public abstract IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false);
    }
}