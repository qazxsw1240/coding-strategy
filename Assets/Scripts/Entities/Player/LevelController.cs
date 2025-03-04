using System;

namespace CodingStrategy.Entities.Player
{
    /// <summary>
    ///     플레이어의 레벨과 경험치를 관리하는 레벨 제어기입니다.
    /// </summary>
    [Obsolete]
    public class LevelController
    {
        /// <summary>
        ///     현재 게임의 플레이어 풀입니다.
        /// </summary>
        private readonly IPlayerPool _playerPool;

        /// <summary>
        ///     현재 게임의 레벨별 필요 경험치를 저장하는 객체입니다.
        /// </summary>
        private readonly IRequiredExp _requiredExp;

        /// <summary>
        ///     레벨 제어기 생성자입니다.
        /// </summary>
        /// <param name="requiredExp">현재 게임에서 생성된 레벨별 필요 경험치 객체를 매개 변수로 입력합니다.</param>
        /// <param name="playerPool">현재 게임에서 생성된 플레이어 풀 객체를 매개 변수로 입력합니다.</param>
        public LevelController(IRequiredExp requiredExp, IPlayerPool playerPool)
        {
            _requiredExp = requiredExp;
            _playerPool = playerPool;
        }

        /// <summary>
        ///     플레이어의 경험치를 증가시킵니다. 만약 증가된 경험치가 레벨별 필요 경험치를 초과할 경우 플레이어의 레벨을 상승시킵니다.
        /// </summary>
        /// <param name="id">플레이어의 아이디를 매개 변수로 입력합니다.</param>
        /// <param name="value">증가시킬 경험치량을 매개 변수로 입력합니다.</param>
        public void IncreaseExp(string id, int value)
        {
            IPlayerDelegate player = _playerPool[id];
            int currentLevel = player.Level;
            int currentExp = player.Exp;
            int totalExp = currentExp + value;

            // 증가된 경험치량이 레벨별 필요 경험치를 초과하지 않을 때까지 반복합니다.
            while (totalExp >= _requiredExp[currentLevel])
            {
                totalExp -= _requiredExp[currentLevel];
                currentLevel++;
            }

            // 레벨 및 경험치를 업데이트합니다.
            player.Level = currentLevel;
            player.Exp = totalExp;
        }
    }
}
