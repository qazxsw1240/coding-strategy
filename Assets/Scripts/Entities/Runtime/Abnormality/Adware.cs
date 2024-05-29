#nullable enable


namespace CodingStrategy.Entities.Runtime.Abnormality
{
    using CodingStrategy.Entities.Player;
    using Robot;
    public class Adware : AbstractAbnormality
    {
        public new static readonly string Name = "애드웨어";
        public Adware(IRobotDelegate robotDelegate, IPlayerDelegate playerDelegate, int value=0)
        :base(Name, robotDelegate, playerDelegate, value)
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
            IAbnormality adware=new Adware(robotDelegate);
            if(keepStatus)
            {
                adware.Value=Value;
            }
            return adware;
        }

        public override void Execute()
        {
            if(_value <= 0)
                return;
            Value--;
        }
    }
}