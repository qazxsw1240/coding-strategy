#nullable enable


namespace CodingStrategy.Runtime
{
    using CodingStrategy.Entities.Robot;

    public interface IRobotAttackStrategy
    {
        public abstract int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target);
    }
}
