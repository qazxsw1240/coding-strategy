#nullable enable

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public class TrojanHorse : AbstractAbnormality
    {
        public TrojanHorse(IRobotDelegate robotDelegate, int value = 0)
            : base(AbnormalityLoader.Load("TrojanHorse"), robotDelegate, value)
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
            IAbnormality trojanHorse = new TrojanHorse(robotDelegate);
            if (keepStatus)
            {
                trojanHorse.Value = Value;
            }
            return trojanHorse;
        }

        public override void Execute()
        {
            if (_value <= 0)
            {
            }
        }
    }
}
