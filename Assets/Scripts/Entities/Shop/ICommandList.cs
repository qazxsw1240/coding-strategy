namespace CodingStrategy.Entities.Shop
{
    using System.Collections.Generic;
    /// <summary>
    /// 현재 게임의 명령어 리스트입니다. 레벨별로 저장할 때 활용합니다.
    /// </summary>
    public interface ICommandList : IList<ICommand>
    {
        /// <summary>
        /// 명령어 리스트에서 랜덤으로 명령어를 추출합니다.
        /// </summary>
        /// <returns>추출된 명령어를 반환합니다.</returns>
        public abstract ICommand SelectRandomCommand();
    }
}