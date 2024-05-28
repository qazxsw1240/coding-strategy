#nullable enable


using System.Collections.Generic;
using CodingStrategy.Entities.Runtime;

namespace CodingStrategy.Entities.BadSector
{
    using Board;
    using Runtime.Statement;
    using Robot;

    public class JumpBadSector : AbstractBadSectorDelegate
    {
        public JumpBadSector(string id, IBoardDelegate boardDelegate, IRobotDelegate installer) : base(id, boardDelegate, installer)
        {
        }

        public override IList<IStatement> Execute(IRobotDelegate target)
        {
            _commandBuilder.Clear();
            _commandBuilder.Append(new MoveCoordinateStatement(target, new Coordinate(0,1)));
            return _commandBuilder.Build();
        }
    }
}