#nullable enable

using System.Collections.Generic;
using System.Linq;

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Placeable;
using CodingStrategy.Entities.Player;
using CodingStrategy.Entities.Robot;

using UnityEngine;

using Random = System.Random;

namespace CodingStrategy.Entities
{
    public class BitDispenser
    {
        private readonly ISet<IBitDelegate> _bits;
        private readonly IBoardDelegate _boardDelegate;
        private readonly IPlayerPool _playerPool;

        public BitDispenser(IBoardDelegate boardDelegate, IPlayerPool playerPool)
        {
            _boardDelegate = boardDelegate;
            _playerPool = playerPool;
            _bits = new HashSet<IBitDelegate>();
        }

        public IList<Coordinate> GetBitPositions(int count)
        {
            IList<Coordinate> coordinates = GetBitPositionCandidates();
            IList<Coordinate> choices = Choice(coordinates, count);
            return choices;
        }

        public void Dispense()
        {
            Dispense(GetBitPositions(_playerPool.Count() * 2));
        }

        public void Dispense(Coordinate position)
        {
            Debug.LogFormat("Bit placed on {0}", position);
            IBitDelegate bitDelegate = new BitDelegateImpl(_playerPool, _boardDelegate, 4);
            _boardDelegate.Add(bitDelegate, position);
            _bits.Add(bitDelegate);
        }

        public void Dispense(IList<Coordinate> positions)
        {
            foreach (Coordinate position in positions)
            {
                Dispense(positions);
            }
        }

        public void ClearTakenBits()
        {
            ISet<IBitDelegate> obsoletes = new HashSet<IBitDelegate>();

            foreach (IBitDelegate bitDelegate in _bits)
            {
                if (bitDelegate.IsTaken)
                {
                    Debug.LogFormat("Remove bit on {0}", bitDelegate.Position);
                    _boardDelegate.Remove(bitDelegate);
                    obsoletes.Add(bitDelegate);
                }
            }

            foreach (IBitDelegate bitDelegate in obsoletes)
            {
                _bits.Remove(bitDelegate);
            }
        }

        public void Clear()
        {
            foreach (IBitDelegate bitDelegate in _bits)
            {
                _boardDelegate.Remove(bitDelegate);
            }

            _bits.Clear();
        }

        private IList<Coordinate> GetCoordinates()
        {
            int width = _boardDelegate.Width;
            int height = _boardDelegate.Height;
            IList<Coordinate> coordinates = new List<Coordinate>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    coordinates.Add(new Coordinate(i, j));
                }
            }

            return coordinates;
        }

        private IList<Coordinate> GetBitPositionCandidates()
        {
            IList<Coordinate> coordinates = GetCoordinates();
            IReadOnlyList<IRobotDelegate> robots = _boardDelegate.Robots;
            IList<Coordinate> robotPositions = robots.ToList().ConvertAll(robot => robot.Position);

            foreach (Coordinate robotPosition in robotPositions)
            {
                coordinates.Remove(robotPosition);
            }

            return coordinates;
        }

        private static IList<Coordinate> Choice(IList<Coordinate> positions, int count)
        {
            Random random = new Random();
            IList<Coordinate> choices = new List<Coordinate>(count);

            for (int i = 0; i < count; i++)
            {
                int target = random.Next(positions.Count - i) + i;
                (positions[i], positions[target]) = (positions[target], positions[i]);
                choices.Add(positions[i]);
            }

            return choices;
        }
    }
}
