using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public class Adware : AbstractAbnormality
    {
        public Adware(IRobotDelegate robotDelegate, int value = 0)
            : base(AbnormalityLoader.Load("Adware"), robotDelegate, value)
        {
        }

        public override int Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_value < 0)
                {
                    _value = 0;
                }
            }
        }

        public override IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false)
        {
            IAbnormality adware = new Adware(robotDelegate);
            if (keepStatus)
            {
                adware.Value = Value;
            }
            return adware;
        }

        public override void Execute()
        {
            if (_value <= 0)
            {
                return;
            }
            Value--;
        }
    }
}
