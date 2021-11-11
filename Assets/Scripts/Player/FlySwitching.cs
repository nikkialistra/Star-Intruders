using System.Collections;
using Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlySwitching : MonoBehaviour
    {
        public bool CanFly => !_landed;
        
        [SerializeField] private bool _landed;
        [SerializeField] private float _landDistance;

        [SerializeField] private float _landingRotateSpeed;
        
        private Rigidbody _rigidBody;
        private Vector3 _landingAngle;

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

            StartCoroutine(RotateSpaceShipToLandingAngle());
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
        }

        private bool ApproximatelyEqual()
        {
            // If difference less than 1 degree
            return Mathf.Abs(Quaternion.Dot(_rigidBody.rotation, Quaternion.Euler(_landingAngle))) >= 0.9999619;
        }

        public void TryTakeoff()
        {
            if (!_landed)
            {
                return;
            }

            _landed = false;
            PlayTakeOffAnimation();
        }

        private void PlayTakeOffAnimation()
        {
            _rigidBody.isKinematic = false;
        }
    }
}
