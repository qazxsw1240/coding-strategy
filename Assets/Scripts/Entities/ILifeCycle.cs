#nullable enable

namespace CodingStrategy.Entities
{
    public interface ILifeCycle
    {
        public abstract void Initialize();

        public abstract bool MoveNext();

        public abstract bool Execute();

        public abstract void Terminate();
    }
}
