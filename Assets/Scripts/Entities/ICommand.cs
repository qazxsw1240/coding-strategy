#nullable enable

using System.Collections.Generic;

using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime;

namespace CodingStrategy.Entities
{
    /// <summary>
    ///     명령어를 나타낸 인터페이스입니다.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     명령어를 구분하는 ID입니다.
        /// </summary>
        public abstract string ID { get; }

        /// <summary>
        ///     명령어의 정보입니다.
        /// </summary>
        public abstract ICommandInfo Info { get; }

        /// <summary>
        ///     실행할 명령어의 Statement가 들어있는 리스트를 반환합니다.
        /// </summary>
        /// <param name="context">명령어를 실행하기 위한 컨텍스트입니다.</param>
        /// <param name="robot">명령어를 실행하는 로봇입니다.</param>
        /// <returns>명령어 Statement 리스트입니다.</returns>
        public abstract IList<IStatement> GetCommandStatements(ICommandContext context, IRobotDelegate robot);

        /// <summary>
        ///     동일한 정보의 명령어를 복제합니다. 현재 명령어의 상태를 반영할지 선택할 수 있습니다.
        ///     기본값으로는 현재 명령어의 상태를 유지한 채 새 명령어를 복제합니다.
        /// </summary>
        /// <param name="keepStatus">현재 명령어의 상태를 반영할지 선택합니다.</param>
        /// <returns>새로 복제된 명령어입니다.</returns>
        public abstract ICommand Copy(bool keepStatus = true);
    }
}
