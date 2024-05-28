#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using Robot;
    public abstract class AbstractAbnormality : IAbnormality
    {
        private readonly string _name;
        protected readonly IRobotDelegate _robotDelegate;
        protected int _value;
        protected AbstractAbnormality(string name, IRobotDelegate robotDelegate)
        {
            _name = name;
            _robotDelegate = robotDelegate;
            _value = 0;
        }
        public virtual string Name => _name;

        public IRobotDelegate RobotDelegate => _robotDelegate;

        public abstract int Value { get; set; }
        public abstract void Execute();
        public abstract IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false);
    }
}