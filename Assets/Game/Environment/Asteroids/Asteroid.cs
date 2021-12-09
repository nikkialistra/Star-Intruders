using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Environment.Asteroids
{
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoBehaviour
    {
        [MinValue(0)]
        [SerializeField] private float _minSpinSpeed;
        [MinValue(0)]
        [SerializeField] private float _maxSpinSpeed;
        [Space, MinValue(0)]
        [SerializeField] private float _minThrust;
        [MinValue(0)]
        [SerializeField] private float _maxThrust;
        [Space, Range(0f, 1f)]
        [SerializeField] private float _chanceToMove;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void AddMovement()
        {
            if (Random.Range(0f, 1f) > _chanceToMove)
            {
                return;
            }
            
            var thrust = Random.Range(_minThrust, _maxThrust);
            var direction = Random.insideUnitCircle.normalized;
            
            var spinSpeed = Random.Range(_minSpinSpeed, _maxSpinSpeed);
            var rotation = Random.rotation.eulerAngles;

            _rigidbody.AddForce(direction * thrust, ForceMode.Impulse);
            _rigidbody.AddTorque(rotation * spinSpeed, ForceMode.Impulse);
        }

        public void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
            _rigidbody.mass *= scale;
        }
    }
}