#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CodingStrategy.Entities.BadSector;
    using CodingStrategy.Entities.Robot;
    using UnityEngine.Events;

    public class BoardDelegateImpl : IBoardDelegate
    {
        private readonly int _width;
        private readonly int _height;
        private readonly IDictionary<IRobotDelegate, RobotPosition> _robotPositions;
        private readonly IDictionary<IBadSectorDelegate, BadSectorPosition> _badSectorPositions;
        private readonly UnityEvent<IRobotDelegate> _robotAddEvents;
        private readonly UnityEvent<IRobotDelegate> _robotRemoveEvents;
        private readonly UnityEvent<IRobotDelegate, Coordinate, Coordinate> _robotChangePositionEvents;
        private readonly UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> _robotChangeDirectionEvents;
        private readonly UnityEvent<IBadSectorDelegate> _badSectorAddEvents;
        private readonly UnityEvent<IBadSectorDelegate> _badSectorRemoveEvents;
        private readonly UnityEvent<IBadSectorDelegate, Coordinate, Coordinate> _badSectorChangePositionEvents;

        public BoardDelegateImpl(int width, int height)
        {
            if (width < 0)
            {
                throw new ArgumentException("Cannot create board delegate with the width:" + width);
            }
            if (height < 0)
            {
                throw new ArgumentException("Cannot create board delegate with the height:" + height);
            }
            _width = width;
            _height = height;
            _robotPositions = new Dictionary<IRobotDelegate, RobotPosition>();
            _badSectorPositions = new Dictionary<IBadSectorDelegate, BadSectorPosition>();
            _robotAddEvents = new UnityEvent<IRobotDelegate>();
            _robotRemoveEvents = new UnityEvent<IRobotDelegate>();
            _robotChangePositionEvents = new UnityEvent<IRobotDelegate, Coordinate, Coordinate>();
            _robotChangeDirectionEvents = new UnityEvent<IRobotDelegate, RobotDirection, RobotDirection>();
            _badSectorAddEvents = new UnityEvent<IBadSectorDelegate>();
            _badSectorRemoveEvents = new UnityEvent<IBadSectorDelegate>();
            _badSectorChangePositionEvents = new UnityEvent<IBadSectorDelegate, Coordinate, Coordinate>();
        }

        public IReadOnlyList<IRobotDelegate> Robots => _robotPositions.Keys.ToList();

        public UnityEvent<IRobotDelegate> OnRobotAdd => _robotAddEvents;

        public UnityEvent<IBadSectorDelegate> OnBadSectorAdd => _badSectorAddEvents;

        public UnityEvent<IRobotDelegate> OnRobotRemove => _robotRemoveEvents;

        public UnityEvent<IBadSectorDelegate> OnBadSectorRemove => _badSectorRemoveEvents;

        public UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition => _robotChangePositionEvents;

        public UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection => _robotChangeDirectionEvents;

        public bool Add(IRobotDelegate robotDelegate, Coordinate position, RobotDirection direction)
        {
            if (IsRobotDelegateExist(robotDelegate))
            {
                return false;
            }
            if (!IsPositionValid(position))
            {
                return false;
            }
            RobotPosition robotPosition = new RobotPosition(position, direction);
            _robotPositions[robotDelegate] = robotPosition;
            _robotAddEvents.Invoke(robotDelegate);
            return true;
        }

        public bool Remove(IRobotDelegate robotDelegate)
        {
            if (!IsRobotDelegateExist(robotDelegate))
            {
                return false;
            }
            _robotPositions.Remove(robotDelegate);
            _robotRemoveEvents.Invoke(robotDelegate);
            return true;
        }

        public bool Add(IBadSectorDelegate badSectorDelegate, Coordinate position)
        {
            if (IsBadSectorDelegateExist(badSectorDelegate))
            {
                return false;
            }
            if (!IsPositionValid(position) || GetBadSectorPosition(position) != null)
            {
                return false;
            }
            BadSectorPosition badSectorPosition = new BadSectorPosition(position);
            _badSectorPositions[badSectorDelegate] = badSectorPosition;
            _badSectorAddEvents.Invoke(badSectorDelegate);
            return true;
        }

        public bool Remove(IBadSectorDelegate badSectorDelegate)
        {
            if (!IsBadSectorDelegateExist(badSectorDelegate))
            {
                return false;
            }
            _badSectorPositions.Remove(badSectorDelegate);
            _badSectorRemoveEvents.Invoke(badSectorDelegate);
            return true;
        }

        public ITileDelegate[,] AsArray() => CreateTileArray();

        public Coordinate GetPosition(IRobotDelegate robotDelegate)
        {
            CheckIfRobotDelegateExists(robotDelegate);
            return _robotPositions[robotDelegate].Position;
        }

        public Coordinate GetPosition(IBadSectorDelegate badSectorDelegate)
        {
            CheckIfBadSectorDelegateExists(badSectorDelegate);
            return _badSectorPositions[badSectorDelegate].Position;
        }

        public RobotDirection GetDirection(IRobotDelegate robotDelegate)
        {
            CheckIfRobotDelegateExists(robotDelegate);
            return _robotPositions[robotDelegate].Direction;
        }

        public bool Place(IRobotDelegate robotDelegate, Coordinate position)
        {
            if (!IsRobotDelegateExist(robotDelegate))
            {
                return false;
            }
            if (!IsPositionValid(position))
            {
                return false;
            }
            RobotPosition robotPosition = _robotPositions[robotDelegate];
            Coordinate previousPosition = robotPosition.Position;
            robotPosition.Position = position;
            _robotChangePositionEvents.Invoke(robotDelegate, previousPosition, position);
            return true;
        }

        public bool Place(IBadSectorDelegate badSectorDelegate, Coordinate position)
        {
            if (!IsBadSectorDelegateExist(badSectorDelegate))
            {
                return false;
            }
            if (!IsPositionValid(position) || GetBadSectorPosition(position) != null)
            {
                return false;
            }
            BadSectorPosition badSectorPosition = _badSectorPositions[badSectorDelegate];
            Coordinate previousPosition = badSectorPosition.Position;
            badSectorPosition.Position = position;
            _badSectorChangePositionEvents.Invoke(badSectorDelegate, previousPosition, position);
            return true;
        }

        public bool Rotate(IRobotDelegate robotDelegate, RobotDirection direction)
        {
            if (!IsRobotDelegateExist(robotDelegate))
            {
                return false;
            }
            RobotPosition robotPosition = _robotPositions[robotDelegate];
            RobotDirection previousDirection = robotPosition.Direction;
            robotPosition.Direction = direction;
            _robotChangeDirectionEvents.Invoke(robotDelegate, previousDirection, direction);
            return true;
        }

        private bool IsPositionValid(Coordinate position)
        {
            int x = position.X;
            int y = position.Y;
            return 0 <= x && x < _width && 0 <= y && y < _height;
        }

        private bool IsRobotDelegateExist(IRobotDelegate robotDelegate) => _robotPositions.ContainsKey(robotDelegate);

        private bool IsBadSectorDelegateExist(IBadSectorDelegate badSectorDelegate) => _badSectorPositions.ContainsKey(badSectorDelegate);

        private void CheckIfRobotDelegateExists(IRobotDelegate robotDelegate)
        {
            if (!IsRobotDelegateExist(robotDelegate))
            {
                throw new ArgumentException($"The robot delegate {robotDelegate.Id} is not placed on this board.");
            }
        }

        private void CheckIfBadSectorDelegateExists(IBadSectorDelegate badSectorDelegate)
        {
            if (!IsBadSectorDelegateExist(badSectorDelegate))
            {
                throw new ArgumentException($"The bad sector delegate {badSectorDelegate.Id} is not placed on this board.");
            }
        }

        private BadSectorPosition? GetBadSectorPosition(Coordinate position)
        {
            foreach (BadSectorPosition badSectorPosition in _badSectorPositions.Values)
            {
                if (badSectorPosition.Position == position)
                {
                    return badSectorPosition;
                }
            }
            return null;
        }

        private ITileDelegate[,] CreateTileArray()
        {
            ITileDelegate[,] tiles = new ITileDelegate[_width, _height];
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    tiles[i, j] = new TileDelegateImpl();
                }
            }
            foreach ((IRobotDelegate robotDelegate, RobotPosition robotPosition) in _robotPositions)
            {
                Coordinate position = robotPosition.Position;
                tiles[position.X, position.Y].Robot.Add(robotDelegate);
            }
            foreach ((IBadSectorDelegate badSectorDelegate, BadSectorPosition badSectorPosition) in _badSectorPositions)
            {
                Coordinate position = badSectorPosition.Position;
                tiles[position.X, position.Y].BadSector = badSectorDelegate;
            }
            return tiles;
        }

        private sealed class RobotPosition
        {
            public Coordinate Position;
            public RobotDirection Direction;

            public RobotPosition(Coordinate position, RobotDirection direction)
            {
                Position = position;
                Direction = direction;
            }
        }

        private sealed class BadSectorPosition
        {
            public Coordinate Position;

            public BadSectorPosition(Coordinate position)
            {
                Position = position;
            }
        }

        private sealed class TileDelegateImpl : ITileDelegate
        {
            private IBadSectorDelegate? _badSector;
            private readonly ISet<IRobotDelegate> _robotDelegates;

            public TileDelegateImpl() : this(null, new HashSet<IRobotDelegate>()) { }

            public TileDelegateImpl(IBadSectorDelegate? badSector, ISet<IRobotDelegate> robotDelegates)
            {
                _badSector = badSector;
                _robotDelegates = robotDelegates;
            }

            public IBadSectorDelegate? BadSector
            {
                get => _badSector;
                set => _badSector = value;
            }
            public ISet<IRobotDelegate> Robot
            {
                get => _robotDelegates;
                set => throw new NotSupportedException();
            }
        }
    }
}
