using System;
using PoolingBenchmark.Features.PerformanceStats.Interfaces;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Features.PerformanceStats.Services
{
    public sealed class FPSMonitor : ITickable, IFPSMonitor
    {
        public event Action<int> OnFPSChanged;

        private float _deltaTime;
        private float _updateTimer;

        private const float UPDATE_INTERVAL = 0.25f;

        public void Tick()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            _updateTimer += Time.unscaledDeltaTime;

            if (_updateTimer < UPDATE_INTERVAL)
                return;

            _updateTimer = 0f;

            int fps = Mathf.RoundToInt(1f / _deltaTime);
            OnFPSChanged?.Invoke(fps);
        }
    }
}