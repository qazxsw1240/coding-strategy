namespace CodingStrategy.Entities.Player
{
    public class RequiredExpImpl : IRequiredExp
    {
        private readonly int[] _requiredExp =
        {
            // 현재 레벨에 따른 레벨별 필요 경험치입니다. 접근하기 쉽도록 인덱스에 맞추어 작성하였습니다.
            0, // level 0
            2, // level 1
            2, // level 2
            6, // level 3
            10, // level 4
            20, // level 5
            36, // level 6
            48, // level 7
            80, // level 8
            84 // level 9
        };

        public int this[int currentLevel]
        {
            get => currentLevel >= 10 ? int.MaxValue : _requiredExp[currentLevel];
        }
    }
}
