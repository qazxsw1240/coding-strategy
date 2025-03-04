using System;

namespace CodingStrategy.Entities.Shop
{
    /// <summary>
    ///     등급별 명령어 리스트입니다.
    /// </summary>
    public class CommandListsByGrade
    {
        // 등급별 명령어 리스트입니다.
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

        /// <summary>
        ///     특정 등급의 명령어 리스트를 반환합니다.
        /// </summary>
        /// <param name="grade">반환 받을 리스트의 등급입니다.</param>
        /// <returns>명령어 리스트를 반환합니다.</returns>
        /// <exception cref="ArgumentException">잘못된 등급일 경우 예외를 발생시킵니다.</exception>
        public CommandListImpl this[int grade]
        {
            get
            {
                switch (grade)
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
