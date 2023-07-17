namespace Core.MemoryPool
{
    public interface IMemoryPool
    {
        void Despawn(object item);
        void Clear();
    }

    public interface IMemoryPool<out TValue> : IMemoryPool
    {
        TValue Spawn();
    }

}