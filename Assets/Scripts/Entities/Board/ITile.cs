#nullable enable


namespace CodingStrategy.Entities.Board
{
    using CodingStrategy.Entities.Obstacle;
    using CodingStrategy.Entities.Robot;

    public interface ITile
    {
        public abstract IObstacle? Obstacle { get; }

        public abstract IRobot? Robot { get; }
    }
}
