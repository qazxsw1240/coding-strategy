namespace CodingStrategy.Entities.Shop
{
    /// <summary>
    /// 인게임 공유 상점입니다. 인게임 명령어 풀과 등급별 명령어 리스트를 통하여 등급별 명령어에 쉽게 접근할 수 있는 클래스입니다.
    /// </summary>
    public class SharedShop
    {
        /// <summary>
        /// 현재 게임의 명령어 풀입니다.
        /// </summary>
        private readonly InGameCommandPool _inGameCommandPool;
        /// <summary>
        /// 등급별 명령어 리스트를 저장합니다.
        /// </summary>
        private readonly CommandListsByGrade _commandListByGrade;
        /// <summary>
        /// 이번 게임의 플레이어 레벨별 리롤 확률입니다.
        /// </summary>
        private IRerollProbability _rerollProbability;
        /// <summary>
        /// 공유 상점 생성자입니다. 현재 게임의 명령어 풀을 인자로 입력받고, 등급별 리스트에 명령어 리스트를 초기화합니다.
        /// </summary>
        /// <param name="inGameCommadPool"></param>
        public SharedShop(InGameCommandPool inGameCommadPool, IRerollProbability rerollProbability)
        {
            _inGameCommandPool = inGameCommadPool;
            _commandListByGrade = new CommandListsByGrade();
            _rerollProbability = rerollProbability;
            Init();
        }
        /// <summary>
        /// 인게임 명령어 풀에서 생성된 명령어를 등급별 리스트에 저장합니다.
        /// </summary>
        private void Init()
        {
            foreach(ICommand command in _inGameCommandPool.Values)
            {
                _commandListByGrade[command.Info.Grade].Add(command);
            }
        }
        /// <summary>
        /// 명령어를 공유 상점에 돌려놓습니다. 반환할 명령어를 해당 등급의 리스트에 추가합니다.
        /// </summary>
        /// <param name="command">반환할 명령어입니다.</param>
        public void ReturnCommand(ICommand command)
        {
            int grade=command.Info.Grade;
            _commandListByGrade[grade].Add(command);
        }
        /// <summary>
        /// 레벨별 확률에 따라 랜덤으로 명령어를 가져옵니다.
        /// </summary>
        /// <param name="level">플레이어의 레벨입니다.</param>
        /// <returns>명령어를 반환합니다.</returns>
        public ICommand GetRandomCommand(int level)
        {
            ICommand command=null;
            while(command==null)
            {
                // 플레이어 레벨에 따른 랜덤 등급을 저장합니다.
                int grade = _rerollProbability.GetRandomGradeFromLevel(level);

                // 추출한 명령어를 저장합니다. 랜덤 등급의 리스트가 비어있을 경우 null을 저장합니다.
                command = _commandListByGrade[grade].SelectRandomCommand();
            }
            return command;
        }
    }
}