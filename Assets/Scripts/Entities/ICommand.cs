#nullable enable


using System;
using System.Collections.Generic;
using CodingStrategy.Entities.Robot;
using CodingStrategy.Entities.Runtime;

namespace CodingStrategy.Entities
{
    /// <summary>
    /// 명령어를 나타낸 인터페이스입니다.
    /// </summary>    
    public interface ICommand
    {
        /// <summary>
        /// 명령어를 구분하는 ID입니다.
        /// </summary>
        public abstract string Id { get; set; }

        /// <summary>
        /// 명령어의 정보입니다.
        /// </summary>
        public abstract ICommandInfo Info { get; }
        public abstract ICommandContext? Context { get; set; }

        /// <summary>
        /// 명령어를 실행합니다. 정상적으로 실행을 완료하면 true를 반환하고, 실패했다면 false를 반환합니다.
        /// </summary>
        /// <param name="args">명령어를 실행하는 데 추가적으로 필요한 매개변수입니다.</param>
        /// <returns>명령어가 정상적으로 실행을 완료하면 true를 반환하고, 실패했다면 false를 반환합니다.</returns>
        [Obsolete]
        public abstract bool Invoke(params object[] args);

        /// <summary>
        /// 실행한 명령어를 취소합니다. 명령어를 정상적으로 취소하였으면 true를 반환하고, 실패했다면 false를 반환합니다.
        /// Invoke 메서드의 반환 결과에 따라 이 메서드가 호출될지 여부가 결정됩니다.
        /// </summary>
        /// <param name="args">명령어를 취소하는 데 추가적으로 필요한 매개변수입니다.</param>
        /// <returns>명령어가 정상적으로 취소됐으면 true를 반환하고, 실패했다면 false를 반환합니다.</returns>
        [Obsolete]
        public abstract bool Revoke(params object[] args);

        /// <summary>
        /// 실행할 명령어의 Statement가 들어있는 리스트를 반환합니다.
        /// </summary>
        /// <param name="robot">명령어를 실행하는 로봇입니다.</param>
        /// <returns>명령어 Statement 리스트입니다.</returns>
        public abstract IList<IStatement> GetCommandStatements(IRobotDelegate robot);

        /// <summary>
        /// 동일한 정보의 명령어를 복제합니다. 현재 명령어의 상태를 반영할지 선택할 수 있습니다.
        /// 기본값으로는 현재 명령어의 상태를 유지한 채 새 명령어를 복제합니다.
        /// </summary>
        /// <param name="keepStatus">현재 명령어의 상태를 반영할지 선택합니다.</param>
        /// <returns>새로 복제된 명령어입니다.</returns>
        public abstract ICommand Copy(bool keepStatus = true);
    }
}
