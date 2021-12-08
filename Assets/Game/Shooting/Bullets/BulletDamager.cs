using System;
using Kernel.Types;
using UnityEngine;

namespace Game.Shooting.Bullets
{
    [RequireComponent(typeof(Collider))]
    public class BulletDamager : MonoBehaviour
    {
        public event Action Hit;
        
        private int _damage;

        public void SetDamage(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Damage must be more than zero");
            }

            _damage = value;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                Hit?.Invoke();
            }
        }
    }
}