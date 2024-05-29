#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using Robot;
    using Player;
    public class Spyware : AbstractAbnormality
    {
        public new static readonly string Name = "스파이웨어";
        public Spyware(IRobotDelegate robotDelegate, int value=0)
        :base(Name, robotDelegate, value)
        {
        }

        public override int Value
        {
            get => _value;
            set
            {
                _value=value;
                if(_value < 0)
                {
                    _value=0;
                }
            }
        }
        public override IAbnormality Copy(IRobotDelegate robotDelegate, bool keepStatus = false)
        {
            IAbnormality spyware=new Spyware(robotDelegate);
            if(keepStatus)
            {
                spyware.Value=Value;
            }
            return spyware;
        }

        public override void Execute()
        {
            if(_value <= 0)
                return;
        }
    }
}