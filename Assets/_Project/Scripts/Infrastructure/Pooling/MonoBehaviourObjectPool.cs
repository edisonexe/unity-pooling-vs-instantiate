using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PoolingBenchmark.Infrastructure.Pooling
{
    public sealed class MonoBehaviourObjectPool<T> where T : MonoBehaviour, IMonoBehaviourPoolable
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool;

        private int _totalCreated;
        private int _reusedCount;

        public T Prefab => _prefab;
        public int TotalCreated => _totalCreated;
        public int ReusedCount => _reusedCount;
        public int AvailableCount => _pool.Count;

        public MonoBehaviourObjectPool(T prefab, Transform parent)
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
                _reusedCount++;
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

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                T item = _pool.Dequeue();
                if (item != null && item.gameObject != null)
                {
                    Object.Destroy(item.gameObject);
                }
            }

            _totalCreated = 0;
            _reusedCount = 0;
        }

        private T CreateInternal()
        {
            T item = Object.Instantiate(_prefab, _parent);
            item.gameObject.SetActive(false);
            _totalCreated++;
            return item;
        }
    }
}