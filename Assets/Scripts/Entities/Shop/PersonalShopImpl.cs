using System;

using CodingStrategy.Entities.Player;

namespace CodingStrategy.Entities.Shop
{
    [Obsolete]
    public class PersonalShopImpl : IPersonalShop
    {
        /// <summary>
        ///     플레이어의 알고리즘입니다.
        /// </summary>
        private readonly IAlgorithm _algorithm;

        /// <summary>
        ///     플레이어의 아이디입니다.
        /// </summary>
        private readonly string _id;

        /// <summary>
        ///     플레이어 레벨 제어기입니다. 경험치 구매시 플레이어의 경험치를 상승시킵니다.
        /// </summary>
        private readonly LevelController _levelController;

        /// <summary>
        ///     플레이어의 정보입니다.
        /// </summary>
        private readonly IPlayerDelegate _player;

        /// <summary>
        ///     이번 게임의 공유 상점입니다.
        /// </summary>
        private readonly SharedShop _sharedShop;

        /// <summary>
        ///     플레이어 상점 생성자입니다.
        /// </summary>
        /// <param name="player">플레이어의 정보입니다.</param>
        /// <param name="rerollProbability">이번 게임의 리롤 확률입니다.</param>
        /// <param name="sharedShop">이번 게임의 공유 상점입니다.</param>
        /// <param name="levelController">이번 게임의 레벨 제어기입니다.</param>
        public PersonalShopImpl(
            IPlayerDelegate player,
            SharedShop sharedShop,
            LevelController levelController
        )
        {
            _player = player;
            _id = player.ID;
            _algorithm = player.Algorithm;
            _sharedShop = sharedShop;
            SellList = new ICommand[5];
            _levelController = levelController;
        }

        /// <summary>
        ///     플레이어 상점의 판매 리스트입니다.
        /// </summary>
        public ICommand[] SellList { get; }

        public void BuyCommand(int sellListIdx)
        {
            // 특정 위치의 커맨드를 추출합니다. 명령어의 비용만큼 플레이어의 재화가 감소합니다.
            ICommand command = GetCommandFromSellList(sellListIdx);

            // 알고리즘에 구매한 명령어를 추가합니다.
            _algorithm.Add(command);
        }

        public void BuyCommand(int sellListIdx, int algorithmIdx)
        {
            // 해당 알고리즘 칸이 비어있지 않을 경우 실행을 취소합니다.
            if (_algorithm[algorithmIdx] != null)
            {
                return;
            }

            // 특정 위치의 커맨드를 추출합니다. 명령어의 비용만큼 플레이어의 재화가 감소합니다.
            ICommand command = GetCommandFromSellList(sellListIdx);

            // 알고리즘에 구매한 명령어를 추가합니다.
            _algorithm.Insert(algorithmIdx, command);
        }

        public void BuyExp(int value)
        {
            // 플레이어 재화가 부족할 경우 실행을 취소합니다.
            int currency = _player.Currency;
            if (currency < value)
            {
                return;
            }

            //플레이어 재화를 감소시키고 경험치를 증가시킵니다.
            _player.Currency -= value;
            _levelController.IncreaseExp(_id, value);
        }

        public void Reroll(int value)
        {
            // 플레이어 재화가 부족할 경우 실행을 취소합니다.
            int currency = _player.Currency;
            if (currency < value)
            {
                return;
            }

            // 플레이어 재화를 감소시킵니다.
            _player.Currency -= value;

            // 판매 리스트를 비웁니다.
            for (int i = 0; i < SellList.Length; i++)
            {
                if (SellList[i] == null)
                {
                    continue;
                }

                // 해당 인덱스의 커맨드를 공유 상점으로 되돌립니다.
                ReturnCommandToSharedShop(SellList[i]);
                SellList[i] = null;
            }

            // 공유 상점에서 커맨드를 추출하여 판매 리스트에 채웁니다.
            for (int i = 0; i < SellList.Length; i++)
            {
                SellList[i] = GetRandomCommandFromSharedShop();
            }
        }

        public void SellCommand(int algorithmIdx)
        {
            // 해당 알고리즘 칸이 비어있을 경우 실행을 취소합니다.
            if (_algorithm[algorithmIdx] == null)
            {
                return;
            }

            // 알고리즘 칸에서 명령어를 추출하여 공유 상점으로 되돌립니다.
            ICommand command = _algorithm[algorithmIdx];
            ReturnCommandToSharedShop(command);

            // 알고리즘에서 해당 명령어를 삭제합니다.
            _algorithm.RemoveAt(algorithmIdx);

            // 명령어의 비용만큼 재화를 더합니다.
            _player.Currency += command.Info.Grade;
        }

        /// <summary>
        ///     명령어를 공유 상점으로 되돌립니다.
        /// </summary>
        /// <param name="command">되돌릴 명령어입니다.</param>
        private void ReturnCommandToSharedShop(ICommand command)
        {
            _sharedShop.ReturnCommand(command);
        }

        /// <summary>
        ///     공유 상점으로부터 랜덤으로 명령어를 추출합니다.
        /// </summary>
        /// <returns>추출한 명령어를 반환합니다.</returns>
        private ICommand GetRandomCommandFromSharedShop()
        {
            return _sharedShop.GetRandomCommand(_player.Level);
        }

        /// <summary>
        ///     판매 리스트에서 명령어를 추출합니다. 명령어의 비용만큼 재화를 감소시킵니다.
        /// </summary>
        /// <param name="sellListIdx">추출할 판매 리스트의 위치입니다.</param>
        /// <returns>추출한 명령어를 반환합니다.</returns>
        private ICommand GetCommandFromSellList(int sellListIdx)
        {
            // 알고리즘에 빈 공간이 없을 경우 실행을 취소합니다.
            if (_algorithm.Count >= _algorithm.Capacity)
            {
                return null;
            }

            // 구매하려는 명령어의 비용이 플레이어 재화보다 높을 경우 실행을 취소합니다.
            int grade = SellList[sellListIdx].Info.Grade;
            if (_player.Currency < grade)
            {
                return null;
            }

            // 구매 위치에 명령어가 존재하지 않을 경우 실행을 취소합니다.
            if (SellList[sellListIdx] == null)
            {
                return null;
            }

            // 판매 리스트의 명령어를 추출합니다.
            ICommand command = SellList[sellListIdx];
            SellList[sellListIdx] = null;

            // 플레이어의 재화를 명령어의 비용만큼 감소시킵니다.
            _player.Currency -= grade;

            return command;
        }
    }
}
