namespace CodingStrategy.Entities.Shop
{
    using CodingStrategy.Entities.Player;
    /// <summary>
    /// 플레이어 상점입니다. 커맨드를 사고 팔거나, 경험치를 구매하고, 리롤을 할 수 있습니다.
    /// </summary>
    public interface IPersonalShop
    {
        /// <summary>
        /// 판매 리스트를 가져옵니다.
        /// </summary>
        public ICommand[] SellList{ get; }
        /// <summary>
        /// 재화를 지불하여 경험치를 구매합니다.
        /// </summary>
        public void BuyExp(int value);
        /// <summary>
        /// 재화를 지불하여 상점에 진열된 명령어를 재설정합니다.
        /// </summary>
        /// <param name="value">지불 재화량입니다.</param>
        public void Reroll(int value);
        /// <summary>
        /// 재화를 지불하여 상점에 진열된 특정 위치의 명령어를 구매합니다.
        /// 알고리즘에 빈공간이 없을 경우 구매가 취소됩니다.
        /// </summary>
        /// <param name="sellListIdx">구매하려는 커맨드의 위치입니다.</param>
        public void BuyCommand(int sellListIdx);
        /// <summary>
        /// 재화를 지불하여 상점에 진열된 특정 위치의 명령어를 플레이어 알고리즘의 특정 위치로 구매합니다.
        /// 알고리즘의 특정 위치에 이미 명령어가 존재하는 경우 구매가 취소됩니다.
        /// </summary>
        /// <param name="sellListIdx">구매하려는 명령어의 위치입니다.</param>
        /// <param name="algorithmIdx">놓으려는 알고리즘의 위치입니다.</param>
        public void BuyCommand(int sellListIdx, int algorithmIdx);
        /// <summary>
        /// 특정 위치의 명령어를 판매합니다.
        /// </summary>
        /// <param name="algorithmIdx">판매하려는 명령어의 위치입니다.</param>
        public void SellCommand(int algorithmIdx);
    }
}