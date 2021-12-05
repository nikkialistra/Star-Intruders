using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using Kernel.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerAnimations))]
    public class FlySwitching : MonoBehaviour
    {
        public bool CanFly { get; private set; } = true;

        [Title("Landing")]
        [MinValue(0)]
        [SerializeField] private float _landingDistance;
        [MinValue(0)]
        [SerializeField] private float _landingRotateSpeed;
        [MinValue(0)]
        [SerializeField] private float _minimumLandingRotateSpeedDuration;
        [MinValue(0)]
        [SerializeField] private float _landingSpeed;
        [MinValue(0)]
        [SerializeField] private float _minimumLandingSpeedDuration;

        [Title("Takeoff")] 
        [MinValue(0)]
        [SerializeField] private float _takeOffAltitude;
        [MinValue(0)]
        [SerializeField] private float _takeoffSpeed;

        [Title("Transform")]
        [InlineEditor, Required]
        [SerializeField] private Transform _spaceShipBottomPoint;

        [Title("Animations")]
        [MinValue(0)]
        [SerializeField] private float _timeToTurnOffEngines;

        private Rigidbody _rigidbody;
        private PlayerAnimations _playerAnimations;
        
        private LandingSurface _landingSurface;
        private Vector3 _landingAngle;

        private bool _isTakingOff;
        private bool _isLanding;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerAnimations = GetComponent<PlayerAnimations>();
        }

        public void TryLand()
        {
            if (!CanFly || _isLanding)
            {
                return;
            }

            if (GetLandingSurfaceWithAngle())
            {
                CanFly = false;
                PlayLandingAnimation();
            }
        }

        public void TryTakeoff()
        {
            if (CanFly || _isLanding)
            {
                return;
            }

            TakeOff();
        }

        private void PlayLandingAnimation()
        {
            if (_isTakingOff)
            {
                return;
            }
            
            _playerAnimations.Land();
            
            _isLanding = true;
            _rigidbody.isKinematic = true;

            StartCoroutine(RotateSpaceShipToLandingAngle());
        }

        private IEnumerator RotateSpaceShipToLandingAngle()
        {
            yield return _rigidbody.DORotate(_landingAngle, GetRotationDuration()).WaitForCompletion();

            StartCoroutine(LandSpaceShipToSurface());
        }

        private IEnumerator LandSpaceShipToSurface()
        {
            GetLandingParameters(out var targetPosition, out var duration);

            yield return _rigidbody.DOMove(targetPosition, duration).WaitForCompletion();
            yield return new WaitForSeconds(_timeToTurnOffEngines);
            
            _playerAnimations.TurnOffEngines();
            _isLanding = false;
        }

        private void TakeOff()
        {
            _isTakingOff = true;
            _playerAnimations.TakeOff();
            StartCoroutine(GainAltitude());
        }

        private IEnumerator GainAltitude()
        {
            GetAltitudeParameters(out var targetPosition, out var duration);

            yield return _rigidbody.DOMove(targetPosition, duration).WaitForCompletion();
            
            _rigidbody.isKinematic = false;
            _isTakingOff = false;

            CanFly = true;
        }

        private bool GetLandingSurfaceWithAngle()
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hit, _landingDistance))
            {
                if (hit.transform.TryGetComponent(out LandingSurface landingSurface))
                {
                    _landingSurface = landingSurface;
                    _landingAngle = landingSurface.transform.rotation.eulerAngles;
                    _landingAngle.y = _rigidbody.rotation.eulerAngles.y;
                    return true;
                }
            }
            return false;
        }

        private float GetRotationDuration()
        {
            var angleDifference = GetAngleDifference(transform.rotation.eulerAngles, _landingAngle);
            var duration = GetDuration(angleDifference, _landingRotateSpeed, _minimumLandingRotateSpeedDuration);
            return duration;
        }

        private float GetAngleDifference(Vector3 firstAngle, Vector3 secondAngle)
        {
            var xDifference = GetAngleDifferenceOnAxis(firstAngle.x, secondAngle.x);
            var zDifference = GetAngleDifferenceOnAxis(firstAngle.z, secondAngle.z);
            
            return xDifference + zDifference;
        }

        private static float GetAngleDifferenceOnAxis(float firstAngle, float secondAngle)
        {
            var difference = firstAngle - secondAngle;
            if (difference > 180)
            {
                difference -= 360;
            }
            if (difference < -180)
            {
                difference += 360;
            }

            return Mathf.Abs(difference);
        }

        private float GetDuration(float value, float speed, float minimumValue = Mathf.NegativeInfinity)
        {
            var duration = value / speed;
            duration = Mathf.Max(duration, minimumValue);
            return duration;
        }

        private void GetLandingParameters(out Vector3 targetPosition, out float duration)
        {
            var closestPointToLandingSurface = GetClosestPointToLandingSurface();

            var distanceToLand = Vector3.Distance(_spaceShipBottomPoint.position, closestPointToLandingSurface);
            duration = GetDuration(distanceToLand, _landingSpeed, _minimumLandingSpeedDuration);

            targetPosition = GetTargetPosition(closestPointToLandingSurface, distanceToLand);
        }

        private void GetAltitudeParameters(out Vector3 targetPosition, out float duration)
        {
            var directionToTakeoff = _rigidbody.transform.up;
            duration = GetDuration(_takeOffAltitude, _takeoffSpeed);

            targetPosition = transform.position + directionToTakeoff * _takeOffAltitude;
        }

        private Vector3 GetClosestPointToLandingSurface()
        {
            var landingSurfaceCollider = _landingSurface.GetComponent<Collider>();
            return landingSurfaceCollider.ClosestPoint(_spaceShipBottomPoint.position);
        }

        private Vector3 GetTargetPosition(Vector3 closestPointToLandingSurface, float distanceToLand)
        {
            var directionToLand = (closestPointToLandingSurface - _spaceShipBottomPoint.position).normalized;
            var targetPosition = transform.position + directionToLand * distanceToLand;
            return targetPosition;
        }
    }
}
