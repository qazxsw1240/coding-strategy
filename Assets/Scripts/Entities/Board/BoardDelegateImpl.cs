#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BadSector;
    using Robot;
    using UnityEngine.Events;

    public class BoardDelegateImpl : IBoardDelegate
    {
        private readonly IDictionary<IRobotDelegate, RobotPosition> _robotPositions;
        private readonly IDictionary<IBadSectorDelegate, BadSectorPosition> _badSectorPositions;
        private readonly IDictionary<IPlaceable, PlaceablePosition> _placeablePositions;
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

            Width = width;
            Height = height;
            _robotPositions = new Dictionary<IRobotDelegate, RobotPosition>();
            _badSectorPositions = new Dictionary<IBadSectorDelegate, BadSectorPosition>();
            _placeablePositions = new Dictionary<IPlaceable, PlaceablePosition>();
            OnRobotAdd = new UnityEvent<IRobotDelegate>();
            OnRobotRemove = new UnityEvent<IRobotDelegate>();
            OnRobotChangePosition = new UnityEvent<IRobotDelegate, Coordinate, Coordinate>();
            OnRobotChangeDirection = new UnityEvent<IRobotDelegate, RobotDirection, RobotDirection>();
            OnBadSectorAdd = new UnityEvent<IBadSectorDelegate>();
            OnBadSectorRemove = new UnityEvent<IBadSectorDelegate>();
            OnPlaceableAdd = new UnityEvent<IPlaceable>();
            OnPlaceableRemove = new UnityEvent<IPlaceable>();
            _badSectorChangePositionEvents = new UnityEvent<IBadSectorDelegate, Coordinate, Coordinate>();
        }

        public int Width { get; }

        public int Height { get; }

        public IReadOnlyList<IRobotDelegate> Robots => _robotPositions.Keys.ToList();

        public UnityEvent<IRobotDelegate> OnRobotAdd { get; }

        public UnityEvent<IBadSectorDelegate> OnBadSectorAdd { get; }

        public UnityEvent<IRobotDelegate> OnRobotRemove { get; }

        public UnityEvent<IBadSectorDelegate> OnBadSectorRemove { get; }

        public UnityEvent<IPlaceable> OnPlaceableAdd { get; }

        public UnityEvent<IPlaceable> OnPlaceableRemove { get; }

        public UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        public UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }

        public ICellDelegate this[Coordinate coordinate]
        {
            get
            {
                ICellDelegate cellDelegate = new CellDelegateImpl();
                foreach ((IRobotDelegate robotDelegate, RobotPosition robotPosition) in _robotPositions)
                {
                    if (robotPosition.Position == coordinate)
                    {
                        cellDelegate.Placeables.Add(robotDelegate);
                    }
                }

                foreach ((IBadSectorDelegate badSectorDelegate, BadSectorPosition badSectorPosition) in
                         _badSectorPositions)
                {
                    if (badSectorPosition.Position == coordinate)
                    {
                        cellDelegate.Placeables.Add(badSectorDelegate);
                    }
                }

                return cellDelegate;
            }
        }

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
            OnRobotAdd.Invoke(robotDelegate);
            return true;
        }

        public bool Remove(IRobotDelegate robotDelegate)
        {
            if (!IsRobotDelegateExist(robotDelegate))
            {
                return false;
            }

            _robotPositions.Remove(robotDelegate);
            OnRobotRemove.Invoke(robotDelegate);
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
            OnBadSectorAdd.Invoke(badSectorDelegate);
            return true;
        }

        public bool Remove(IBadSectorDelegate badSectorDelegate)
        {
            if (!IsBadSectorDelegateExist(badSectorDelegate))
            {
                return false;
            }

            _badSectorPositions.Remove(badSectorDelegate);
            OnBadSectorRemove.Invoke(badSectorDelegate);
            return true;
        }

        public bool Add(IPlaceable placeable, Coordinate position)
        {
            if (IsPlaceableExist(placeable))
            {
                return false;
            }

            if (!IsPositionValid(position))
            {
                return false;
            }

            PlaceablePosition placeablePosition = new PlaceablePosition(position);
            _placeablePositions[placeable] = placeablePosition;
            OnPlaceableAdd.Invoke(placeable);
            return true;
        }

        public bool Remove(IPlaceable placeable)
        {
            if (!IsPlaceableExist(placeable))
            {
                return false;
            }

            _placeablePositions.Remove(placeable);
            OnPlaceableRemove.Invoke(placeable);
            return true;
        }


        public IRobotDelegate? GetRobotDelegate(Coordinate coordinate)
        {
            foreach ((IRobotDelegate robotDelegate, RobotPosition position) in _robotPositions)
            {
                if (position.Position == coordinate)
                {
                    return robotDelegate;
                }
            }

            return null;
        }

        public IBadSectorDelegate? GetBadSectorDelegate(Coordinate coordinate)
        {
            foreach ((IBadSectorDelegate badSectorDelegate, BadSectorPosition position) in _badSectorPositions)
            {
                if (position.Position == coordinate)
                {
                    return badSectorDelegate;
                }
            }

            return null;
        }

        public IList<IPlaceable> GetPlaceables(Coordinate coordinate)
        {
            IList<IPlaceable> placeables = new List<IPlaceable>();

            foreach ((IPlaceable placeable, PlaceablePosition position) in _placeablePositions)
            {
                if (position.Position == coordinate)
                {
                    placeables.Add(placeable);
                }
            }

            return placeables;
        }

        public ICellDelegate[,] AsArray()
        {
            return CreateCellArray();
        }

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

        public Coordinate GetPosition(IPlaceable placeable)
        {
            CheckIfPlaceableExists(placeable);
            return _placeablePositions[placeable].Position;
        }

        public RobotDirection GetDirection(IRobotDelegate robotDelegate)
        {
            CheckIfRobotDelegateExists(robotDelegate);
            return _robotPositions[robotDelegate].Direction;
        }

        public bool Place(IRobotDelegate robotDelegate, Coordinate position)
        {
            // if (!IsRobotDelegateExist(robotDelegate))
            // {
            //     return false;
            // }

            if (!IsPositionValid(position))
            {
                return false;
            }

            RobotPosition robotPosition = _robotPositions[robotDelegate];
            Coordinate previousPosition = robotPosition.Position;
            robotPosition.Position = position;
            OnRobotChangePosition.Invoke(robotDelegate, previousPosition, position);
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
            OnRobotChangeDirection.Invoke(robotDelegate, previousDirection, direction);
            return true;
        }

        private bool IsPositionValid(Coordinate position)
        {
            int x = position.X;
            int y = position.Y;
            return 0 <= x && x < Width && 0 <= y && y < Height;
        }

        private bool IsRobotDelegateExist(IRobotDelegate robotDelegate)
        {
            return _robotPositions.ContainsKey(robotDelegate);
        }

        private bool IsBadSectorDelegateExist(IBadSectorDelegate badSectorDelegate)
        {
            return _badSectorPositions.ContainsKey(badSectorDelegate);
        }

        private bool IsPlaceableExist(IPlaceable placeable)
        {
            return _placeablePositions.ContainsKey(placeable);
        }

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
                throw new ArgumentException(
                    $"The bad sector delegate {badSectorDelegate.Id} is not placed on this board.");
            }
        }

        private void CheckIfPlaceableExists(IPlaceable placeable)
        {
            if (!IsPlaceableExist(placeable))
            {
                throw new ArgumentException(
                    $"The placeable {placeable.GetHashCode()}({placeable.GetType().Name}) is not placed on this board.");
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

        private ICellDelegate[,] CreateCellArray()
        {
            ICellDelegate[,] tiles = new ICellDelegate[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    tiles[i, j] = new CellDelegateImpl();
                }
            }

            foreach ((IRobotDelegate robotDelegate, RobotPosition robotPosition) in _robotPositions)
            {
                Coordinate position = robotPosition.Position;
                tiles[position.X, position.Y].Placeables.Add(robotDelegate);
            }

            foreach ((IBadSectorDelegate badSectorDelegate, BadSectorPosition badSectorPosition) in _badSectorPositions)
            {
                Coordinate position = badSectorPosition.Position;
                tiles[position.X, position.Y].Placeables.Add(badSectorDelegate);
            }

            foreach ((IPlaceable placeable, PlaceablePosition placeablePosition) in _placeablePositions)
            {
                Coordinate position = placeablePosition.Position;
                tiles[position.X, position.Y].Placeables.Add(placeable);
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

        private sealed class PlaceablePosition
        {
            public Coordinate Position;

            public PlaceablePosition(Coordinate position)
            {
                Position = position;
            }
        }

        private sealed class CellDelegateImpl : ICellDelegate
        {
            private readonly ISet<IRobotDelegate> _robotDelegates;

            public CellDelegateImpl() : this(null, new HashSet<IRobotDelegate>()) {}

            private CellDelegateImpl(IBadSectorDelegate? badSector, ISet<IRobotDelegate> robotDelegates)
            {
                BadSector = badSector;
                _robotDelegates = robotDelegates;
                Placeables = new List<IPlaceable>();
            }

            public IBadSectorDelegate? BadSector { get; set; }

            public ISet<IRobotDelegate> Robot
            {
                get => _robotDelegates;
                set => throw new NotSupportedException();
            }

            public IList<IPlaceable> Placeables { get; }
        }
    }
}
