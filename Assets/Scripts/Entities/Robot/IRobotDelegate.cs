#nullable enable


using System.Collections.Generic;
using CodingStrategy.Entities.Runtime;

namespace CodingStrategy.Entities.Robot
{
    using UnityEngine.Events;

    /// <summary>
    /// 로봇의 정보를 관리하는 딜리게이트입니다.
    /// </summary>
    public interface IRobotDelegate : IGameEntity, IPlaceable
    {
        public abstract Coordinate[] Vectors { get; }
        /// <summary>
        /// 로봇이 지닌 알고리즘입니다. 알고리즘에는 턴마다 실행할 명령어가 순서대로 저장돼 있습니다.
        /// </summary>
        public abstract IAlgorithm Algorithm { get; }

        /// <summary>
        /// 로봇이 위치한 좌표입니다.
        /// </summary>
        public new abstract Coordinate Position { get; set; }

        /// <summary>
        /// 로봇이 바라보고 있는 방향입니다.
        /// </summary>
        public abstract RobotDirection Direction { get; set; }

        /// <summary>
        ///  로봇의 체력입니다.
        /// </summary>
        public abstract int HealthPoint { get; set; }

        /// <summary>
        /// 로봇의 에너지입니다.
        /// </summary>
        public abstract int EnergyPoint { get; set; }

        /// <summary>
        /// 로봇의 방어력입니다.
        /// </summary>
        public abstract int ArmorPoint { get; set; }

        /// <summary>
        /// 로봇의 공격력입니다.
        /// </summary>
        public abstract int AttackPoint { get; set; }

        /// <summary>
        /// 로봇이 앞이나 뒤로 일정한 수의 셀을 이동합니다.
        /// </summary>
        /// <param name="count">
        ///     값의 부호로 방향을 정합니다. 양수면 앞으로 이동하고, 음수면 뒤로 이동합니다.
        ///     로봇의 방향에는 영향을 주지 않습니다.
        /// </param>
        /// <returns></returns>
        public abstract bool Move(int count);

        /// <summary>
        /// 로봇이 해당 좌표로 이동합니다.
        /// </summary>
        /// <param name="position">로봇이 이동할 좌표입니다.</param>
        /// <returns></returns>
        public abstract bool Move(Coordinate position);

        /// <summary>
        /// 로봇이 왼쪽이나 오른쪽으로 일정한 수만큼 회전합니다.
        /// </summary>
        /// <param name="count">
        ///     값의 부호로 회전 방향을 정합니다. 양수면 오른쪽으로 회전하고, 음수면 왼쪽으로 이동합니다.
        ///     로봇의 위치에는 영향을 주지 않습니다.
        /// </param>
        /// <returns></returns>
        public abstract bool Rotate(int count);

        /// <summary>
        /// 로봇이 특정 방향으로 회전합니다.
        /// </summary>
        /// <param name="direction">로봇이 바라볼 방향입니다.</param>
        /// <returns></returns>
        public abstract bool Rotate(RobotDirection direction);

        /// <summary>
        /// 로봇이 특정 좌표에 존재하는 모든 로봇을 공격합니다.
        /// </summary>
        /// <param name="strategy">
        ///     로봇이 다른 로봇을 공격할 때 사용되는 공격력 계산법입니다.
        /// </param>
        /// <param name="relativePosition">
        ///     로봇의 공격이 적용되는 좌표입니다.
        /// </param>
        /// <returns></returns>
        public abstract bool Attack(IRobotAttackStrategy strategy, params Coordinate[] relativePosition);

        /// <summary>
        /// 로봇이 위치가 변할 때 발생하는 이벤트입니다. 
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, Coordinate, Coordinate> OnRobotChangePosition { get; }

        /// <summary>
        /// 로봇의 방향이 변할 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, RobotDirection, RobotDirection> OnRobotChangeDirection { get; }

        /// <summary>
        /// 로봇이 공격을 수행할 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, IList<Coordinate>> OnRobotAttack { get; }

        /// <summary>
        /// 로봇의 체력이 변할 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, int, int> OnHealthPointChange { get; }

        /// <summary>
        /// 로봇의 에너지가 변할 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, int, int> OnEnergyPointChange { get; }

        /// <summary>
        /// 로봇의 방어력이 변할 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, int, int> OnArmorPointChange { get; }

        /// <summary>
        /// 로봇의 공격력이 변할 때 발생하는 이벤트입니다.
        /// </summary>
        public abstract UnityEvent<IRobotDelegate, int, int> OnAttackPointChange { get; }
    }
}
