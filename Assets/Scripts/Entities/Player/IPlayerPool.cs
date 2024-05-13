namespace CodingStrategy.Entities.Player
{
    /// <summary>
    /// 플레이어 풀입니다. 플레이어의 아이디로 해당 플레이어의 상태에 접근할 수 있습니다.
    /// </summary>
    public interface IPlayerPool : IObjectPool<IPlayerDelegate> {}
}
