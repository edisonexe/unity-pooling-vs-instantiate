using System;
using System.Collections.Generic;

namespace PoolingBenchmark.Infrastructure.Pooling
{
    public sealed class PocoObjectPool<T> where T : class, IPocoPoolable
    {
        private readonly Queue<T> _pool;
        private readonly Func<T> _factoryMethod;

        public int TotalCreated { get; private set; }
        public int AvailableCount => _pool.Count;

        public PocoObjectPool(Func<T> factoryMethod, int initialCapacity)
        {
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));
            _pool = new Queue<T>(initialCapacity);
        }
        
        public void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _pool.Enqueue(CreateNewItem());
            }
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            
            return CreateNewItem();
        }

        public void Return(T item)
        {
            if (item == null) return;

            item.Reset();
            _pool.Enqueue(item);
        }

        private T CreateNewItem()
        {
            TotalCreated++;
            return _factoryMethod.Invoke();
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}