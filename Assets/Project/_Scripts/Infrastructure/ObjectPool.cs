using System;
using System.Collections.Generic;
using PoolingBenchmark.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PoolingBenchmark.Infrastructure
{
    public sealed class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool;

        public T Prefab => _prefab;
        public int TotalCreated { get; private set; }
        public int ReusedCount { get; private set; }
        public int AvailableCount => _pool.Count;

        public ObjectPool(T prefab, Transform parent)
        {
            _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _pool = new Queue<T>(512);
        }

        public void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _pool.Enqueue(CreateInternal());
            }
        }

        public T Get()
        {
            T item;
            if (_pool.Count > 0)
            {
                item = _pool.Dequeue();
                ReusedCount++;
            }
            else
            {
                item = CreateInternal();
            }
            
            item.OnSpawn();
            return item;
        }

        public void Return(T item)
        {
            if (!item) return;
            
            item.OnDespawn();
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        private T CreateInternal()
        {
            T item = Object.Instantiate(_prefab, _parent);
            item.gameObject.SetActive(false);
            TotalCreated++;
            return item;
        }
    }
}