using System;
using Unity.VisualScripting;

namespace CodingStrategy.Entities.Shop
{
    public class CommandListsByGrade
    {
        private readonly CommandListImpl _commandListGrade1;
        private readonly CommandListImpl _commandListGrade2;
        private readonly CommandListImpl _commandListGrade3;
        private readonly CommandListImpl _commandListGrade4;
        private readonly CommandListImpl _commandListGrade5;
        public CommandListsByGrade()
        {
            _commandListGrade1 = new CommandListImpl();
            _commandListGrade2 = new CommandListImpl();
            _commandListGrade3 = new CommandListImpl();
            _commandListGrade4 = new CommandListImpl();
            _commandListGrade5 = new CommandListImpl();
        }

        public CommandListImpl this[int grade]
        {
            get
            {
                switch(grade)
                {
                    case 1:
                        return _commandListGrade1;
                    case 2:
                        return _commandListGrade2;
                    case 3: 
                        return _commandListGrade3;
                    case 4:
                        return _commandListGrade4;
                    case 5:
                        return _commandListGrade5;
                    default:
                        throw new ArgumentException();
                }
            }
        }
    }
}