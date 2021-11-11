using System;
using Entities;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlySwitching : MonoBehaviour
    {
        public bool CanFly => !_landed;
        
        [SerializeField] private bool _landed;
        [SerializeField] private float _landDistance;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void TryLand()
        {
            if (_landed)
            {
                return;
            }

            if (CheckForLandingSurface())
            {
                _landed = true;
                PlayLandingAnimation();
            }
        }

        private bool CheckForLandingSurface()
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hit, _landDistance))
            {
                if (hit.transform.TryGetComponent(out LandingSurface landingSurface))
                {
                    return true;
                }
            }
            return false;
        }

        private void PlayLandingAnimation()
        {
            _rigidbody.isKinematic = true;
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
            _rigidbody.isKinematic = false;
        }
    }
}
