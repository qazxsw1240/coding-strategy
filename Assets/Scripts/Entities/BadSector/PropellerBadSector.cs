using System.Collections.Generic;

using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.BadSector
{
    public class PropellerBadSector : AbstractBadSectorDelegate
    {
        public PropellerBadSector(
            string id,
            IBoardDelegate boardDelegate,
            IRobotDelegate installer)
            : base(id, boardDelegate, installer)
        {
        }

        public override string Explanation => "발동시 바라보는 방향의 반대 방향으로 1칸 이동합니다.";

        public override IList<IStatement> Execute(IRobotDelegate target)
        {
            return new List<IStatement>
            {
                new MoveCoordinateStatement(target, new Coordinate(0, -1))
            };
        }
    }
}
