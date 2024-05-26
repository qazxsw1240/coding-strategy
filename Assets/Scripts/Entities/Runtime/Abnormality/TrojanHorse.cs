#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using Robot;
    public class TrojanHorse : AbstractAbnormality
    {
        public new static readonly string Name = "트로이 목마";
        public TrojanHorse(IRobotDelegate robotDelegate) : base(Name, robotDelegate)
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
            IAbnormality trojanHorse=new TrojanHorse(robotDelegate);
            if(keepStatus)
            {
                trojanHorse.Value=Value;
            }
            return trojanHorse;
        }

        public override void Execute()
        {
            return;
        }
    }
}