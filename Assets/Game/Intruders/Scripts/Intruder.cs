using System;
using Kernel.Entity;
using UnityEngine;
using Zenject;

namespace Game.Intruders
{
    [RequireComponent(typeof(IntruderControl))]
    [RequireComponent(typeof(EntityHealth))]
    public class Intruder : MonoBehaviour, IPoolable<IntruderSpecs, Vector3, IMemoryPool>, IIntruderData, IDisposable
    {
        private IntruderSpecs _specs;

        private IntruderControl _control;
        private EntityHealth _health;
        
        private IMemoryPool _pool;

        private void Awake()
        {
            _control = GetComponent<IntruderControl>();
            _health = GetComponent<EntityHealth>();
        }

        private void Start()
        {
            _health.Died += Dispose;
        }

        public IntruderSpecs GetSpecs()
        {
            return _specs;
        }

        public void OnSpawned(IntruderSpecs specs, Vector3 position, IMemoryPool pool)
        {
            _pool = pool;
            _specs = specs;
            transform.position = position;
            
            _control.HeadToTarget();
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<IntruderSpecs, Vector3, Intruder>
        {
        }
    }
}