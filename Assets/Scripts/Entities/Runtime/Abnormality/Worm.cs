#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public class Worm : AbstractAbnormality
    {
        public new const string Name = "웜";

        public Worm(IRobotDelegate robotDelegate, int value = 0) : base(Name, robotDelegate, value) {}

        public override int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (_value > 64)
                {
                    _value = 64;
                }
                else if (_value < 0)
                {
                    _value = 0;
                }
            }
        }

        public override IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false)
        {
            IAbnormality worm = new Worm(robotDelegate);

            if (keepStatus)
            {
                worm.Value = Value;
            }

            return worm;
        }

        public override void Execute()
        {
            if (_value <= 0)
            {
                return;
            }
            Value *= 2;
        }
    }
}
