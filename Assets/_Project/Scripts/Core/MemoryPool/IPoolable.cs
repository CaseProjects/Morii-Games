namespace Core.MemoryPool
{
    public interface IPoolable<in TParam1, in TParam2> : IPoolable
    {
        void OnSpawned(TParam1 param1, TParam2 pool);
    }

    public interface IPoolable<in TParam1> : IPoolable
    {
        void OnSpawned(TParam1 pool);
    }


    public interface IPoolable
    {
        void OnDespawned();
    }
}