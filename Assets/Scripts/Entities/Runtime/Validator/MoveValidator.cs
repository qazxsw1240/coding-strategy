#nullable enable

using System.Collections.Generic;
using System.Linq;

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;

using UnityEngine;

namespace CodingStrategy.Entities.Runtime.Validator
{
    public class MoveValidator : IExecutionValidator
    {
        public bool IsValid(IBoardDelegate boardDelegate)
        {
            ICellDelegate[,] cellDelegates = boardDelegate.AsArray();
            for (int i = 0; i < cellDelegates.GetLength(0); i++)
            {
                for (int j = 0; j < cellDelegates.GetLength(1); j++)
                {
                    ICellDelegate cellDelegate = cellDelegates[i, j];

                    if (cellDelegate.Placeables.Count(p => p is IRobotDelegate) <= 1)
                    {
                        continue;
                    }

                    Debug.LogFormat("Collision Found at {0}", new Coordinate(i, j));
                    return false;
                }
            }

            return true;
        }

        public IList<IRobotDelegate> GetInvalidRobots(IBoardDelegate boardDelegate)
        {
            ICellDelegate[,] cellDelegates = boardDelegate.AsArray();
            for (int i = 0; i < cellDelegates.GetLength(0); i++)
            {
                for (int j = 0; j < cellDelegates.GetLength(1); j++)
                {
                    ICellDelegate cellDelegate = cellDelegates[i, j];
                    IList<IRobotDelegate> robotDelegates = cellDelegate.Placeables
                       .Where(p => p is IRobotDelegate)
                       .Cast<IRobotDelegate>()
                       .ToList();

                    if (robotDelegates.Count <= 1)
                    {
                        continue;
                    }
                    Debug.LogFormat("Collision Found at {0}", new Coordinate(i, j));
                    return robotDelegates;
                }
            }
            return new List<IRobotDelegate>();
        }
    }
}
