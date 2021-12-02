using System.Collections;
using DG.Tweening;
using Kernel.Types;
using UnityEngine;

namespace Game.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerAnimations))]
    public class FlySwitching : MonoBehaviour
    {
        public bool CanFly { get; private set; }
        
        [Header("Landing")]
        [SerializeField] private bool _landed;
        [SerializeField] private float _landingDistance;
        [SerializeField] private float _landingRotateSpeed;
        [SerializeField] private float _minimumLandingRotateSpeedDuration;
        [SerializeField] private float _landingSpeed;
        [SerializeField] private float _minimumLandingSpeedDuration;

        [Header("Takeoff")] 
        [SerializeField] private float _takeOffAltitude;
        [SerializeField] private float _takeoffSpeed;

        [Header("Transform")]
        [SerializeField] private Transform _spaceShipBottomPoint;

        [SerializeField] private float _timeToTurnOffEngines;
        
        
        private Rigidbody _rigidBody;
        private PlayerAnimations _playerAnimations;
        
        private LandingSurface _landingSurface;
        private Vector3 _landingAngle;
        
        private Coroutine _landingAnimation;
        private Coroutine _takeOffAnimation;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _playerAnimations = GetComponent<PlayerAnimations>();
            CanFly = !_landed;
        }

        public void TryLand()
        {
            if (_landed)
            {
                return;
            }

            if (GetLandingSurfaceWithAngle())
            {
                _landed = true;
                CanFly = false;
                PlayLandingAnimation();
            }
        }

        private bool GetLandingSurfaceWithAngle()
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hit, _landingDistance))
            {
                if (hit.transform.TryGetComponent(out LandingSurface landingSurface))
                {
                    _landingSurface = landingSurface;
                    _landingAngle = landingSurface.transform.rotation.eulerAngles;
                    _landingAngle.y = _rigidBody.rotation.eulerAngles.y;
                    return true;
                }
            }
            return false;
        }

        private void PlayLandingAnimation()
        {
            if (_takeOffAnimation != null)
            {
                StopCoroutine(_takeOffAnimation);
            }
            
            _playerAnimations.Land();
            _rigidBody.isKinematic = true;

            _landingAnimation = StartCoroutine(RotateSpaceShipToLandingAngle());
        }

        private IEnumerator RotateSpaceShipToLandingAngle()
        {
            var duration = GetRotationDuration();
            
            yield return _rigidBody.DORotate(_landingAngle, duration).WaitForCompletion();

            _landingAnimation = StartCoroutine(LandSpaceShipToSurface());
        }

        private IEnumerator LandSpaceShipToSurface()
        {
            var closestPointToLandingSurface = GetClosestPointToLandingSurface();

            var distanceToLand = Vector3.Distance(_spaceShipBottomPoint.position, closestPointToLandingSurface);
            var duration = GetDuration(distanceToLand, _landingSpeed, _minimumLandingSpeedDuration);

            var targetPosition = GetTargetPosition(closestPointToLandingSurface, distanceToLand);

            yield return _rigidBody.DOMove(targetPosition, duration).WaitForCompletion();

            yield return new WaitForSeconds(_timeToTurnOffEngines);
            _playerAnimations.TurnOffEngines();
        }

        private Vector3 GetTargetPosition(Vector3 closestPointToLandingSurface, float distanceToLand)
        {
            var directionToLand = (closestPointToLandingSurface - _spaceShipBottomPoint.position).normalized;
            var targetPosition = transform.position + directionToLand * distanceToLand;
            return targetPosition;
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

        private Vector3 GetClosestPointToLandingSurface()
        {
            var landingSurfaceCollider = _landingSurface.GetComponent<Collider>();
            return landingSurfaceCollider.ClosestPoint(_spaceShipBottomPoint.position);
        }

        public void TryTakeoff()
        {
            if (!_landed)
            {
                return;
            }

            if (_landingAnimation != null)
            {
                StopCoroutine(_landingAnimation);
            }

            _landed = false;
            
            PlayTakeOffAnimation();
        }

        private void PlayTakeOffAnimation()
        {
            _playerAnimations.TakeOff();
            _takeOffAnimation = StartCoroutine(GainAltitude());
        }

        private IEnumerator GainAltitude()
        {
            var directionToTakeoff = _rigidBody.transform.up;
            var duration = GetDuration(_takeOffAltitude, _takeoffSpeed);

            var targetPosition = transform.position + directionToTakeoff * _takeOffAltitude;

            yield return _rigidBody.DOMove(targetPosition, duration).WaitForCompletion();

            _rigidBody.isKinematic = false;
            CanFly = true;
        }

        private float GetDuration(float value, float speed, float minimumValue = Mathf.NegativeInfinity)
        {
            var duration = value / speed;
            duration = Mathf.Max(duration, minimumValue);
            return duration;
        }
    }
}
