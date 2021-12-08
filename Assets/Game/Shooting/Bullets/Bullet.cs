using System;
using UnityEngine;
using Zenject;

namespace Game.Shooting.Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BulletDamager))]
    public class Bullet : MonoBehaviour, IPoolable<BulletSpecs, IMemoryPool>, IDisposable
    {
        private float _lifetime;
        private int _damage;
        
        private float _currentLifetime;

        private IMemoryPool _pool;

        private BulletDamager _damager;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _damager = GetComponent<BulletDamager>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _damager.Hit += Dispose;
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
            _damager.SetDamage(bulletSpecs.Damage);

            _rigidbody.velocity = bulletSpecs.Direction * bulletSpecs.MoveSpeed;

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