namespace CodingStrategy.Factory
{
    public class RobotDelegateCreateStrategy: IRobotDelegateCreateStrategy
    {
        public int HealthPoint => 3;
        public int EnergyPoint => 3;
        public int ArmorPoint => 0;
        public int AttackPoint => 0;
    }
}
