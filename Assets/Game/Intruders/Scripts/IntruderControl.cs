using Game.Environment.TransitZone;
using UnityEngine;
using Zenject;

namespace Game.Intruders.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class IntruderControl : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private TransitZoneDestination _transitZoneDestination;

        private Vector3 _targetPoint;
        
        private Rigidbody _rigidbody;

        [Inject]
        public void Construct(TransitZoneDestination transitZoneDestination)
        {
            _transitZoneDestination = transitZoneDestination;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void HeadToTarget()
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