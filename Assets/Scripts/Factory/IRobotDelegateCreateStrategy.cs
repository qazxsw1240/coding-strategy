#nullable enable


namespace CodingStrategy.Factory
{
    public interface IRobotDelegateCreateStrategy
    {
        public abstract int HealthPoint { get; }

        public abstract int EnergyPoint { get; }

        public abstract int ArmorPoint { get; }

        public abstract int AttackPoint { get; }
    }
}
