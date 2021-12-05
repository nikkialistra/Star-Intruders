using Game.Environment.TransitZone;
using UnityEngine;

namespace Game.Intruders.Scripts
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
            HeadToTarget();
        }

        private void HeadToTarget()
        {
            _targetPoint = _transitZoneDestination.GetRandomPointInBox();
            transform.LookAt(_targetPoint);
        }

        private void FixedUpdate()
        {
            var newPosition = Vector3.MoveTowards(transform.position, _targetPoint, _speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPosition);
            transform.LookAt(_targetPoint);
        }
    }
}