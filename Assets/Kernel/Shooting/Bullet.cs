using UnityEngine;

namespace Kernel.Shooting
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        private float _lifetime;
        private float _currentLifetime;
        private float _damage;

        private bool _isInitialized;

        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void Initialize(float lifetime, float damage, float moveSpeed, Vector3 direction)
        {
            _lifetime = lifetime;
            _damage = damage;

            _rigidBody.velocity = direction * moveSpeed;
        }

        private void FixedUpdate()
        {
            UpdateCurrentLifetime();
        }

        private void UpdateCurrentLifetime()
        {
            _currentLifetime += Time.fixedDeltaTime;
            if (_currentLifetime > _lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}