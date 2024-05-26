#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using Robot;
    public class Spyware : AbstractAbnormality
    {
        public new static readonly string Name = "스파이웨어";
        public Spyware(IRobotDelegate robotDelegate) : base(Name, robotDelegate)
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
            return;
        }
    }
}