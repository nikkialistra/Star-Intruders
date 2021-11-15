﻿using CameraControls;
using Entities;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(ShakeCameraOffset))]
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private float _shakeOnAsteroidCollision;
        
        private ShakeCameraOffset _shakeCameraOffset;

        private bool _isShook;

        private void Awake()
        {
            _shakeCameraOffset = GetComponent<ShakeCameraOffset>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<Asteroid>() != null)
            {
                _isShook = true;
                _shakeCameraOffset.ApplyShake(_shakeOnAsteroidCollision);
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (_isShook)
            {
                _shakeCameraOffset.ApplyShake(_shakeOnAsteroidCollision);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.GetComponent<Asteroid>() != null)
            {
                _isShook = false;
            }
        }
    }
}