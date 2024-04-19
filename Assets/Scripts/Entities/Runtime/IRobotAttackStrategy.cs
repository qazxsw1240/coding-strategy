#nullable enable


namespace CodingStrategy.Runtime
{
    using CodingStrategy.Entities.Robot;

    /// <summary>
    /// 로봇의 공격과 피해를 계산하는 데에 활용되는 인터페이스입니다.
    /// </summary>
    public interface IRobotAttackStrategy
    {
        /// <summary>
        /// 로봇의 공격량과 피해량을 계산합니다.
        /// </summary>
        /// <param name="attacker">공격하는 로봇의 딜리게이트입니다.</param>
        /// <param name="target">공격받는 로봇의 딜리게이트입니다.</param>
        /// <returns>계산된 공격량이 반환됩니다.</returns>
        public abstract int CalculateAttackPoint(IRobotDelegate attacker, IRobotDelegate target);
    }
}
