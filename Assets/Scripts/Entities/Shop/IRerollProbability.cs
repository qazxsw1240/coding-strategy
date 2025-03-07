namespace CodingStrategy.Entities.Shop
{
    /// <summary>
    ///     플레이어 레벨별 리롤 확률에 따라 랜덤으로 명령어 등급을 반환하는 인터페이스입니다.
    /// </summary>
    public interface IRerollProbability
    {
        /// <summary>
        ///     명령어의 등급을 랜덤으로 반환합니다. 플레이어 레벨이 높을 수록 높은 등급이 등장할 확률이 높아집니다.
        /// </summary>
        /// <param name="level">플레이어 레벨입니다.</param>
        /// <returns>랜덤 등급을 반환합니다.</returns>
        public int GetRandomGradeFromLevel(int level);
    }
}
