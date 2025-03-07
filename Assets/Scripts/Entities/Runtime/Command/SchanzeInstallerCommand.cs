#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.BadSector;
using CodingStrategy.Entities.Board;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime.Statement;

namespace CodingStrategy.Entities.Runtime.Command
{
    public class SchanzeInstallerCommand : AbstractCommand
    {
        private static int _installNum;

        private readonly List<Coordinate> _coordinates = new List<Coordinate>();

        public SchanzeInstallerCommand(int enhancedLevel = 1)
            : base(CommandLoader.Load(13), enhancedLevel, 1)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            return keepStatus ? new SchanzeInstallerCommand(Info.EnhancedLevel) : new SchanzeInstallerCommand();
        }

        private static IBadSectorDelegate CreateJumpBadSector(
            IBoardDelegate boardDelegate,
            IRobotDelegate robotDelegate)
        {
            return new JumpBadSector($"{robotDelegate.ID}-{_installNum++}", boardDelegate, robotDelegate);
        }

        protected override void AddStatementOnLevel1(IRobotDelegate robotDelegate)
        {
            _coordinates.Add(new Coordinate(0, 1));
            _commandBuilder.Append(new PointerStatement(robotDelegate, CreateJumpBadSector, _coordinates));
        }

        protected override void AddStatementOnLevel2(IRobotDelegate robotDelegate) {}

        protected override void AddStatementOnLevel3(IRobotDelegate robotDelegate)
        {
            _commandBuilder.Clear();
            _coordinates.Add(new Coordinate(-1, 1));
            _coordinates.Add(new Coordinate(1, 1));
            _commandBuilder.Append(new PointerStatement(robotDelegate, CreateJumpBadSector, _coordinates));
        }
    }
}
