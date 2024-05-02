#nullable enable


namespace CodingStrategy.Entities.Board
{
    using System.Collections.Generic;
    using BadSector;
    using Robot;

    /// <summary>
    /// 셀의 정보를 담고 있는 딜리게이트입니다.
    /// </summary>
    public interface ICellDelegate
    {
        /// <summary>
        /// 셀에 설치된 배드섹터입니다. 설치된 배드섹터가 없으면 null이 반환됩니다.
        /// </summary>
        public abstract IBadSectorDelegate? BadSector { get; set; }

        /// <summary>
        /// 셀에 배치된 로봇 컬렉션입니다.
        /// </summary>
        public abstract ISet<IRobotDelegate> Robot { get; set; }

        /// <summary>
        /// 셀에 설치된 Placeable 객체를 리스트로 반환합니다.
        /// </summary>
        public abstract IReadOnlyList<IPlaceable> Placeables { get; }
    }
}
