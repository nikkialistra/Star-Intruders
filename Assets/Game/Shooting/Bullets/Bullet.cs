using System;
using UnityEngine;
using Zenject;

namespace Game.Shooting.Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BulletDamager))]
    public class Bullet : MonoBehaviour, IPoolable<BulletSpecs, Vector3, Quaternion, IMemoryPool>, IDisposable
    {
        private float _lifetime;
        private int _damage;
        
        private float _currentLifetime;
        
        private BulletDamager _damager;
        private Rigidbody _rigidbody;
        
        private IMemoryPool _pool;

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

        public void OnSpawned(BulletSpecs bulletSpecs, Vector3 position, Quaternion rotation, IMemoryPool pool)
        {
            _pool = pool;
            
            transform.position = position;
            transform.rotation = rotation;
            
            GetParametersFromSpecs(bulletSpecs);

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

        private void GetParametersFromSpecs(BulletSpecs bulletSpecs)
        {
            _lifetime = bulletSpecs.Lifetime;
            _damager.SetDamage(bulletSpecs.Damage);

            _rigidbody.velocity = bulletSpecs.Direction * bulletSpecs.MoveSpeed;
        }

        private void UpdateCurrentLifetime()
        {
            _currentLifetime += Time.fixedDeltaTime;
            if (_currentLifetime > _lifetime)
            {
                Dispose();
            }
        }

        public class Factory : PlaceholderFactory<BulletSpecs, Vector3, Quaternion, Bullet>
        {
        }
    }
}