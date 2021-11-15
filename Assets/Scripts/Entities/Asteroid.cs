using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float _minSpinSpeed;
        [SerializeField] private float _maxSpinSpeed;
        [Space]
        [SerializeField] private float _minThrust;
        [SerializeField] private float _maxThrust;
        [Space] 
        [Range(0f, 1f)]
        [SerializeField] private float _chanceToMove;

        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void SetMass()
        {
            _rigidBody.mass = transform.localScale.x;
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

            _rigidBody.AddForce(direction * thrust, ForceMode.Impulse);
            _rigidBody.AddTorque(rotation * spinSpeed, ForceMode.Impulse);
        }
    }
}