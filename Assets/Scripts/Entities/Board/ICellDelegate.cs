using System;
using System.Collections.Generic;

using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Robot;

namespace CodingStrategy.Entities.Board
{
    /// <summary>
    ///     셀의 정보를 담고 있는 딜리게이트입니다.
    /// </summary>
    public interface ICellDelegate
    {
        /// <summary>
        ///     셀에 설치된 배드섹터입니다. 설치된 배드섹터가 없으면 null이 반환됩니다.
        /// </summary>
        [Obsolete]
        public abstract IBadSectorDelegate BadSector { get; set; }

        /// <summary>
        ///     셀에 배치된 로봇 컬렉션입니다.
        /// </summary>
        [Obsolete]
        public abstract ISet<IRobotDelegate> Robot { get; set; }

        /// <summary>
        ///     셀에 설치된 Placeable 객체를 리스트로 반환합니다.
        /// </summary>
        public abstract IList<IPlaceable> Placeables { get; }
    }
}
