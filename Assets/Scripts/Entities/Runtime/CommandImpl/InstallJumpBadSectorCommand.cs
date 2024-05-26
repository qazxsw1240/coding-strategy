#nullable enable


namespace CodingStrategy.Entities.Runtime.CommandImpl
{
    using System.Collections.Generic;
    using CodingStrategy.Entities.BadSector;
    using CodingStrategy.Entities.Board;
    using Robot;
    using Statement;

    public class InstallJumpBadSectorCommand : AbstractCommand
    {
        public static int installNum=0;
        private readonly CommandBuilder _commandBuilder=new();

        public InstallJumpBadSectorCommand(string id="14", string name="점프대 설치", int enhancedLevel=1, int grade=1)
        : base(id, name, enhancedLevel, grade)
        {
        }

        public override ICommand Copy(bool keepStatus = true)
        {
            if(!keepStatus)
            {
                return new CoinMiningCommand();
            }
            return new CoinMiningCommand(Id, Info.Name, Info.EnhancedLevel, Info.Grade);
        }

        public override IList<IStatement> GetCommandStatements(IRobotDelegate robot)
        {
            _commandBuilder.Clear();
            List<Coordinate> coordinates=new List<Coordinate>();
            coordinates.Add(new Coordinate(0,2));
            _commandBuilder.Append(new PointerStatement(robot, InstallJumpBadSector, coordinates));
            return _commandBuilder.Build();
        }
        public IBadSectorDelegate InstallJumpBadSector(IBoardDelegate boardDelegate, IRobotDelegate robotDelegate)
        {
            return new JumpBadSector(robotDelegate.Id+"-"+installNum++, boardDelegate, robotDelegate);
        }

        public override bool Invoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public override bool Revoke(params object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}