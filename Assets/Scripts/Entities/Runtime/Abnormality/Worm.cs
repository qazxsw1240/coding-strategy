#nullable enable

using System;

using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Runtime.Abnormality
{
    public class Worm : AbstractAbnormality
    {
        public Worm(IRobotDelegate robotDelegate, int value = 0)
            : base(AbnormalityLoader.Load("Worm"), robotDelegate, value)
        {
        }

        public override int Value
        {
            get => _value;
            set => _value = Math.Min(Math.Max(value, 0), 64);
        }

        public override IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false)
        {
            return keepStatus
                ? new Worm(robotDelegate, _value)
                : new Worm(robotDelegate);
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
