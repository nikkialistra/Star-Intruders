using System;
using UnityEngine;
using Zenject;

namespace Game.Shooting.Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour, IPoolable<BulletSpecs, IMemoryPool>, IDisposable
    {
        private float _lifetime;
        private float _damage;
        
        private float _currentLifetime;

        private IMemoryPool _pool;
        
        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            UpdateCurrentLifetime();
        }

        public void OnSpawned(BulletSpecs bulletSpecs, IMemoryPool pool)
        {
            _pool = pool;

            transform.position = bulletSpecs.Position;
            transform.rotation = bulletSpecs.Rotation;

            _lifetime = bulletSpecs.Lifetime;
            _damage = bulletSpecs.Damage;

            _rigidBody.velocity = bulletSpecs.Direction * bulletSpecs.MoveSpeed;

            _currentLifetime = 0;
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        private void UpdateCurrentLifetime()
        {
            _currentLifetime += Time.fixedDeltaTime;
            if (_currentLifetime > _lifetime)
            {
                Dispose();
            }
        }

        public class Factory : PlaceholderFactory<BulletSpecs, Bullet>
        {
        }
    }
}