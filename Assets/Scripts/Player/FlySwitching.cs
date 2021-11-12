using System.Collections;
using Entities;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlySwitching : MonoBehaviour
    {
        public bool CanFly { get; private set; }
        
        [Header("Landing")]
        [SerializeField] private bool _landed;
        [SerializeField] private float _landingDistance;
        [SerializeField] private float _landingRotateSpeed;
        [SerializeField] private float _landingSpeed;
        
        [Header("Takeoff")] 
        [SerializeField] private float _takeOffAltitude;
        [SerializeField] private float _takeoffSpeed;

        [Header("Transform")]
        [SerializeField] private Transform _spaceShipBottomPoint;
        
        private Rigidbody _rigidBody;
        
        private LandingSurface _landingSurface;
        private Vector3 _landingAngle;
        
        private Coroutine _landingAnimation;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
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
            _rigidBody.isKinematic = true;

            _landingAnimation = StartCoroutine(RotateSpaceShipToLandingAngle());
        }

        private IEnumerator RotateSpaceShipToLandingAngle()
        {
            while (!ApproximatelyEqual())
            {
                var currentRotation = Quaternion.Lerp(_rigidBody.rotation, Quaternion.Euler(_landingAngle), 
                    _landingRotateSpeed * Time.fixedDeltaTime);
                _rigidBody.rotation = currentRotation;

                yield return new WaitForFixedUpdate();
            }
            _landingAnimation = StartCoroutine(LandSpaceShipToSurface());
        }

        private IEnumerator LandSpaceShipToSurface()
        {
            var closestPointToLandingSurface = GetClosestPointToLandingSurface();

            var distanceToLand = Vector3.Distance(_spaceShipBottomPoint.position, closestPointToLandingSurface);
            var directionToLand = (closestPointToLandingSurface - _spaceShipBottomPoint.position).normalized;
            
            var passedDistance = 0f;
            Vector3 deltaMove;
            
            while (passedDistance < distanceToLand)
            {
                deltaMove = directionToLand * (_landingSpeed * Time.fixedDeltaTime);
                _rigidBody.MovePosition(_rigidBody.position + deltaMove);
                passedDistance += deltaMove.magnitude;

                yield return new WaitForFixedUpdate();
            }
        }

        private Vector3 GetClosestPointToLandingSurface()
        {
            var landingSurfaceCollider = _landingSurface.GetComponent<Collider>();
            return landingSurfaceCollider.ClosestPoint(_spaceShipBottomPoint.position);
        }

        private bool ApproximatelyEqual()
        {
            // If the difference is less than 1 degree
            return Mathf.Abs(Quaternion.Dot(_rigidBody.rotation, Quaternion.Euler(_landingAngle))) >= 0.9999619;
        }

        public void TryTakeoff()
        {
            if (!_landed)
            {
                return;
            }
            
            StopCoroutine(_landingAnimation);
            _landed = false;
            
            PlayTakeOffAnimation();
        }

        private void PlayTakeOffAnimation()
        {
            StartCoroutine(GainAltitude());
        }

        private IEnumerator GainAltitude()
        {
            var directionToTakeoff = _rigidBody.transform.up;
            
            var passedDistance = 0f;
            Vector3 deltaMove;
            
            while (passedDistance < _takeOffAltitude)
            {
                deltaMove = directionToTakeoff * (_takeoffSpeed * Time.fixedDeltaTime);
                _rigidBody.MovePosition(_rigidBody.position + deltaMove);
                passedDistance += deltaMove.magnitude;

                yield return new WaitForFixedUpdate();
            }
            
            _rigidBody.isKinematic = false;
            CanFly = true;
        }
    }
}
