using System.Collections.Generic;

namespace CodingStrategy.Entities
{
    /// <summary>
    /// 게임 전체에 사용될 명령어를 관리하는 명령어 풀입니다. 모든 명령어는 상태가 유일하고,
    /// 각 게임마다 명령어를 생성하여 관리하도록 합니다.
    /// </summary>
    public interface ICommandPool : IReadOnlyDictionary<string, ICommand>
    {
        /// <summary>
        /// 게임에 필요한 명령어를 생성합니다.
        /// </summary>
        /// <param name="id">명령어 생성에 필요한 명령어  ID입니다.</param>
        /// <returns>새로 생성된 명령어를 반환합니다.</returns>
        public abstract ICommand Create(string id);
    }
}
