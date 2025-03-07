using System.Collections.Generic;

using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Robot;

using UnityEngine.Events;

namespace CodingStrategy.Entities.Board
{
    /// <summary>
    ///     보드의 정보를 지닌 딜리게이트입니다.
    /// </summary>
    public interface IBoardDelegate
    {
        /// <summary>
        ///     보드의 가로 크기입니다.
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        ///     보드의 세로 크기입니다.
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        ///     보드에 추가된 로봇 컬렉션입니다.
        /// </summary>
        public abstract IReadOnlyList<IRobotDelegate> Robots { get; }

        /// <summary>
        ///     좌푯값으로 셀의 정보를 반환합니다.
        /// </summary>
        /// <param name="coordinate">셀의 정보를 확인할 좌표입니다.</param>
        /// <returns>좌표에 해당하는 ICellDelegate 인스턴스를 반환합니다.</returns>

        public abstract ICellDelegate this[Coordinate coordinate] { get; }

        /// <summary>
        ///     로봇이 추가됐을 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate> OnRobotAdd { get; }

        /// <summary>
        ///     배드섹터가 추가됐을 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorAdd { get; }

        /// <summary>
        ///     로봇이 보드에서 제거됐을 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate> OnRobotRemove { get; }

        /// <summary>
        ///     배드섹터가 보드에서 제거됐을 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IBadSectorDelegate> OnBadSectorRemove { get; }

        public abstract UnityEvent<IPlaceable> OnPlaceableAdd { get; }

        public abstract UnityEvent<IPlaceable> OnPlaceableRemove { get; }

        /// <summary>
        ///     로봇의 위치가 바뀌었을 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        /// <summary>
        ///     로봇이 바라보는 방향이 바뀌었을 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }

        /// <summary>
        ///     보드에 로봇을 추가합니다. 중복된 로봇이 보드에 존재하면 추가하지 않습니다.
        /// </summary>
        /// <param name="robotDelegate">추가할 로봇 딜리게이트입니다.</param>
        /// <param name="position">로봇이 추가될 때의 위치입니다.</param>
        /// <param name="direction">로봇이 추가될 때의 방향입니다.</param>
        /// <returns>성공적으로 추가되면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public abstract bool Add(IRobotDelegate robotDelegate, Coordinate position, RobotDirection direction);

        /// <summary>
        ///     보드에서 로봇을 제거합니다.
        /// </summary>
        /// <param name="robotDelegate">제거할 로봇 딜리게이트입니다.</param>
        /// <returns>성공적으로 제거하면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public abstract bool Remove(IRobotDelegate robotDelegate);

        /// <summary>
        ///     새 베드섹터를 보드에 추가합니다. 중복된 배드섹터가 보드에 존재하면 추가하지 않습니다.
        /// </summary>
        /// <param name="badSectorDelegate">추가할 배드섹터 딜리게이트입니다.</param>
        /// <param name="position">배드섹터의 추가될 떄의 위치입니다.</param>
        /// <returns>성공적으로 추가하면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public abstract bool Add(IBadSectorDelegate badSectorDelegate, Coordinate position);

        /// <summary>
        ///     보드에서 배드섹터를 제거합니다.
        /// </summary>
        /// <param name="badSectorDelegate">제거할 배드섹터 딜리게이트입니다.</param>
        /// <returns>성공적으로 제거하면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public abstract bool Remove(IBadSectorDelegate badSectorDelegate);

        public abstract bool Add(IPlaceable placeable, Coordinate position);

        public abstract bool Remove(IPlaceable placeable);

        /// <summary>
        ///     로봇의 좌표를 반환합니다.
        /// </summary>
        /// <param name="robotDelegate">좌표를 확인할 로봇 딜리게이트입니다.</param>
        /// <returns>로봇의 좌표입니다.</returns>
        public abstract Coordinate GetPosition(IRobotDelegate robotDelegate);

        /// <summary>
        ///     배드섹터의 좌표를 반환합니다.
        /// </summary>
        /// <param name="badSectorDelegate">좌표를 확인할 배드섹터 딜리게이트입니다.</param>
        /// <returns>배드섹터의 좌표입니다.</returns>
        public abstract Coordinate GetPosition(IBadSectorDelegate badSectorDelegate);

        public abstract Coordinate GetPosition(IPlaceable placeable);

        /// <summary>
        ///     로봇이 바라보는 방향을 반환합니다.
        /// </summary>
        /// <param name="robotDelegate">방향을 확인할 로봇 딜리게이트입니다.</param>
        /// <returns>로봇이 바라보는 방향입니다.</returns>
        public abstract RobotDirection GetDirection(IRobotDelegate robotDelegate);

        /// <summary>
        ///     로봇의 위치를 옮깁니다.
        /// </summary>
        /// <param name="robotDelegate">위치를 옮길 로봇 딜리게이트입니다.</param>
        /// <param name="position">로봇이 옮길 위치입니다.</param>
        /// <returns>성공적으로 옮기면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public abstract bool Place(IRobotDelegate robotDelegate, Coordinate position);

        /// <summary>
        ///     로봇이 바라보는 방향을 변경합니다.
        /// </summary>
        /// <param name="robotDelegate">방향을 변경할 로봇 딜리게이트입니다.</param>
        /// <param name="direction">로봇이 바라볼 방향입니다.</param>
        /// <returns>성공적으로 변경하면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public abstract bool Rotate(IRobotDelegate robotDelegate, RobotDirection direction);

        /// <summary>
        ///     배드섹터의 위치를 옮깁니다.
        /// </summary>
        /// <param name="badSectorDelegate">위치를 옮길 배드섹터 딜리게이트입니다.</param>
        /// <param name="position">배드섹터를 옮길 위치입니다.</param>
        /// <returns>성공적으로 옮기면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public abstract bool Place(IBadSectorDelegate badSectorDelegate, Coordinate position);

        public abstract IRobotDelegate GetRobotDelegate(Coordinate coordinate);

        public abstract IBadSectorDelegate GetBadSectorDelegate(Coordinate coordinate);

        public abstract IList<IPlaceable> GetPlaceables(Coordinate coordinate);

        /// <summary>
        ///     보드의 각 셀 정보를 배열로 반환합니다.
        /// </summary>
        /// <returns>셀 딜리게이트가 담긴 2차원 배열이 반환됩니다.</returns>
        public abstract ICellDelegate[,] AsArray();
    }
}
