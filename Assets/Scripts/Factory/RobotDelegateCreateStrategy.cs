namespace CodingStrategy.Factory
{
    public class RobotDelegateCreateStrategy : IRobotDelegateCreateStrategy
    {
        public int HealthPoint
        {
            get { return 3; }
        }

        public int EnergyPoint
        {
            get { return 3; }
        }

        public int ArmorPoint
        {
            get { return 0; }
        }

        public int AttackPoint
        {
            get { return 0; }
        }
    }
}
