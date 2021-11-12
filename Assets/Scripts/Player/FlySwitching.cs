using System.Collections;
using Entities;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlySwitching : MonoBehaviour
    {
        public bool CanFly => !_landed;
        
        [Header("Landing")]
        [SerializeField] private bool _landed;
        [SerializeField] private float _landDistance;
        [SerializeField] private float _landingRotateSpeed;
        [SerializeField] private float _landingSpeed;

        [Header("Transform")]
        [SerializeField] private Transform _spaceShipBottomPoint;
        
        private Rigidbody _rigidBody;
        
        private LandingSurface _landingSurface;
        private Vector3 _landingAngle;
        
        private Coroutine _landingAnimation;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
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
                PlayLandingAnimation();
            }
        }

        private bool GetLandingSurfaceWithAngle()
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hit, _landDistance))
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
            var distanceToLand = Vector3.Distance(_spaceShipBottomPoint.position, _landingSurface.transform.position);
            var directionToLand = (_landingSurface.transform.position - _spaceShipBottomPoint.position).normalized;
            
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

            _landed = false;
            StopCoroutine(_landingAnimation);
            
            PlayTakeOffAnimation();
        }

        private void PlayTakeOffAnimation()
        {
            _rigidBody.isKinematic = false;
        }
    }
}
