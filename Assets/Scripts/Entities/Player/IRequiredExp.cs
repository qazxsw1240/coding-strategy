namespace CodingStrategy.Entities.Player
{
    /// <summary>
    /// 레벨별 필요 경험치를 반환하는 인터페이스입니다.
    /// </summary>
    public interface IRequiredExp
    {
        /// <summary>
        /// 현재 레벨에서 다음 레벨까지 필요한 총 경험치를 반환합니다.
        /// </summary>
        /// <param name="currentLevel">현재 레벨을 매개 변수로 입력합니다.</param>
        /// <returns>다음 레벨까지 필요한 총 경험치를 반환합니다.</returns>
        public abstract int this[int currentLevel] { get; }
    }
}