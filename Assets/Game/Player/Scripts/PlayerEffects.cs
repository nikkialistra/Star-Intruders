﻿using Game.Cameras.Scripts;
using Game.Environment.Asteroids;
using UnityEngine;
using Zenject;

namespace Game.Player.Scripts
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private float _shakeOnAsteroidCollision;

        private ShakeCameraOffset _shakeCameraOffset;

        private bool _isShook;

        [Inject]
        public void Construct(ShakeCameraOffset shakeCameraOffset)
        {
            _shakeCameraOffset = shakeCameraOffset;
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