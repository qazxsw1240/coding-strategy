#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public class Spyware : AbstractAbnormality
    {
        public Spyware(IRobotDelegate robotDelegate, int value = 0)
            : base(AbnormalityLoader.Load("Spyware"), robotDelegate, value)
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
            IAbnormality spyware = new Spyware(robotDelegate);
            if (keepStatus)
            {
                spyware.Value = Value;
            }
            return spyware;
        }

        public override void Execute()
        {
            if (_value <= 0)
            {
            }
        }
    }
}
