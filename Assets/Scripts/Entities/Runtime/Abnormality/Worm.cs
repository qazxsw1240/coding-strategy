#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using Robot;
    public class Worm : AbstractAbnormality
    {
        public new static readonly string Name = "웜";
        public Worm(IRobotDelegate robotDelegate) : base(Name, robotDelegate)
        {
        }

        public override int Value
        {
            get => _value;
            set
            {
                _value=value;
                if(_value>64)
                {
                    _value=64;
                }
                else if(_value < 0)
                {
                    _value=0;
                }
            }
        }

        public override IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false)
        {
            IAbnormality worm=new Worm(robotDelegate);
            if(keepStatus)
            {
                worm.Value=Value;
            }
            return worm;
        }

        public override void Execute()
        {
            if(_value <= 0)
                return;
            Value*=2;
        }
    }
}