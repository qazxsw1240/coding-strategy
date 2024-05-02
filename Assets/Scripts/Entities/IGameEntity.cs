#nullable enable


namespace CodingStrategy.Entities
{
    using System;
    /// <summary>
    /// 게임에 배치되는 요소입니다. IGameEntity는 ID로 인스턴스를 구분합니다.
    /// </summary>
    public interface IGameEntity : IComparable<IGameEntity>
    {
        /// <summary>
        /// IGameEntity의 고유한 ID입니다.
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// 하위 IGameEntity로 타입을 변환합니다. 타입 변환에 실패하면 예외가 발생합니다.
        /// </summary>
        /// <typeparam name="TEntity">변환할 하위 타입입니다.</typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">타입 변환에 실패하면 발생할 예외입니다.</exception>
        public TEntity As<TEntity>() where TEntity : class, IGameEntity
            => this as TEntity ?? throw new InvalidCastException();

        /// <summary>
        /// 기본적으로 구현된 비교 메서드입니다. ID로 구분합니다.
        /// </summary>
        /// <param name="other">비교할 다른 IGameEntity입니다.</param>
        /// <returns></returns>
        public new int CompareTo(IGameEntity other) => Id.CompareTo(other.Id);
    }
}
