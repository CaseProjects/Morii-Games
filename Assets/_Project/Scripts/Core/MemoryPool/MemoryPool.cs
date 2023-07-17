using System;
using System.Collections.Generic;

namespace Core.MemoryPool
{
    public class MemoryPool<TValue> : MemoryPoolBase<TValue>, IMemoryPool<TValue>
    {
        public MemoryPool(Func<TValue> createFunc, Action<TValue> actionOnGet, Action<TValue> actionOnRelease,
            Action<TValue> actionOnDestroy) : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy)
        {
        }
    }


    public class MemoryPoolBase<TValue> : IMemoryPool, IDisposable
    {
        private readonly Stack<TValue> _stack;
        private readonly Func<TValue> _createFunc;
        private readonly Action<TValue> _actionOnGet;
        private readonly Action<TValue> _actionOnRelease;
        private readonly Action<TValue> _actionOnDestroy;

        public int Count => _stack.Count;

        protected MemoryPoolBase(
            Func<TValue> createFunc,
            Action<TValue> actionOnGet,
            Action<TValue> actionOnRelease,
            Action<TValue> actionOnDestroy)
        {
            _stack = new Stack<TValue>(50);
            _createFunc = createFunc;
            _actionOnGet = actionOnGet;
            _actionOnRelease = actionOnRelease;
            _actionOnDestroy = actionOnDestroy;
        }


        public void Clear()
        {
            if (_actionOnDestroy != null)
                foreach (var obj in _stack)
                    _actionOnDestroy(obj);

            _stack.Clear();
        }

        public TValue Spawn()
        {
            var item = GetItem();

            _actionOnGet?.Invoke(item);
            return item;
        }

        public void Despawn(TValue item)
        {
            _actionOnRelease?.Invoke(item);
            _stack.Push(item);
        }

        public void Despawn(object item)
        {
            if (item is TValue component) Despawn(component);
        }

        public void Dispose()
        {
            Clear();
        }

        protected TValue GetItem()
        {
            var item = _stack.Count == 0 ? _createFunc() : _stack.Pop();
            return item;
        }
    }
}