using System.Collections.Generic;
using PoolingBenchmark.Features.Targets;
using UnityEngine;

namespace PoolingBenchmark.Infrastructure.Collections
{
    public sealed class SpatialGrid
    {
        private readonly float _cellSize;
        
        // ключ — упакованные координаты ячейки (x, z), значение — список целей в ней
        private readonly Dictionary<long, List<TargetEntity>> _buckets = new(1024);
        private readonly List<List<TargetEntity>> _allocatedLists = new(1024);
        private int _listPoolIndex;

        public SpatialGrid(float cellSize)
        {
            _cellSize = cellSize;
        }

        public void Clear()
        {
            _buckets.Clear();
            _listPoolIndex = 0;
        }

        public void Insert(TargetEntity target)
        {
            Vector3 pos = target.Position;
            long key = GetKey(pos.x, pos.z);

            if (!_buckets.TryGetValue(key, out List<TargetEntity> list))
            {
                list = GetOrCreateList();
                _buckets[key] = list;
            }
            list.Add(target);
        }

        public List<TargetEntity> GetCell(Vector3 position)
        {
            long key = GetKey(position.x, position.z);
            _buckets.TryGetValue(key, out List<TargetEntity> list);
            return list;
        }

        private long GetKey(float x, float z)
        {
            long cellX = Mathf.FloorToInt(x / _cellSize);
            long cellZ = Mathf.FloorToInt(z / _cellSize);
            return (cellX << 32) | (cellZ & 0xFFFFFFFFL); // упаковка двух координат в один long
        }

        private List<TargetEntity> GetOrCreateList()
        {
            if (_listPoolIndex >= _allocatedLists.Count)
            {
                var newList = new List<TargetEntity>(32);
                _allocatedLists.Add(newList);
                _listPoolIndex++;
                return newList;
            }

            List<TargetEntity> pooledList = _allocatedLists[_listPoolIndex];
            pooledList.Clear();
            _listPoolIndex++;
            return pooledList;
        }
    }
}