using System;

namespace CodingStrategy.Entities.Shop
{
    public class RerollProbabilityImpl : IRerollProbability
    {
        /// <summary>
        ///     플레이어 레벨별 명령어 등급 등장 확률입니다.
        /// </summary>
        private readonly int[][] probability =
        {
            new[] { 0, 0, 0, 0, 0 }, //level 0
            new[] { 100, 0, 0, 0, 0 }, //level 1
            new[] { 100, 0, 0, 0, 0 }, //level 2
            new[] { 75, 25, 0, 0, 0 }, //level 3
            new[] { 55, 30, 15, 0, 0 }, //level 4
            new[] { 45, 33, 20, 2, 0 }, //level 5
            new[] { 30, 40, 25, 5, 0 }, //level 6
            new[] { 19, 35, 35, 10, 1 }, //level 7
            new[] { 18, 25, 36, 18, 3 }, //level 8
            new[] { 10, 20, 25, 35, 10 }, //level 9
            new[] { 5, 10, 20, 40, 25 } //level 10
        };

        private readonly Random random = new Random();

        public int GetRandomGradeFromLevel(int level)
        {
            // 1~100의 난수를 발생시킵니다.
            int randomValue = random.Next(100) + 1;
            for (int i = 0; i < 5; i++)
            {
                // 발생한 난수가 등급 등장 확률보다 클 경우 해당 값만큼 난수를 감소시키고 다음 인덱스로 이동합니다.
                if (randomValue > probability[level][i])
                {
                    randomValue -= probability[level][i];
                    continue;
                }
                // 발생한 난수가 등급 등장 확률보다 작을 경우 해당 등급을 반환합니다.
                return i + 1;
            }

            // 난수가 1~100의 값이 아닐 경우 반환될 값입니다.
            return -1;
        }
    }
}
