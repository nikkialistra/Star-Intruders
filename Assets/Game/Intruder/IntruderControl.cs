using Game.Environment.TransitZone;
using UnityEngine;

namespace Game.Intruder
{
    [RequireComponent(typeof(Rigidbody))]
    public class IntruderControl : MonoBehaviour
    {
        [SerializeField] private TransitZoneDestination _transitZoneDestination;
        
        [SerializeField] private float _speed;

        private Vector3 _targetPoint;
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _targetPoint = _transitZoneDestination.RandomPointInBox();
        }

        private void FixedUpdate()
        {
            var newPosition = Vector3.MoveTowards(transform.position, _targetPoint, _speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPosition);
        }
    }
}