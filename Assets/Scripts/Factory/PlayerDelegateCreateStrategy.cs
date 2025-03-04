using CodingStrategy.Entities;

namespace CodingStrategy.Factory
{
    public class PlayerDelegateCreateStrategy : IPlayerDelegateCreateStrategy
    {
        public PlayerDelegateCreateStrategy(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public IAlgorithm Algorithm
        {
            get { return new AlgorithmImpl(1); }
        }

        public int HealthPoint
        {
            get { return 3; }
        }

        public int Level
        {
            get { return 1; }
        }

        public int Exp
        {
            get { return 0; }
        }

        public int Currency
        {
            get { return 0; }
        }
    }
}
