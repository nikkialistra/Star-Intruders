using Game.Asteroids;
using Kernel.CameraControls;
using UnityEngine;

namespace Kernel.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private float _shakeOnAsteroidCollision;

        [SerializeField] private ShakeCameraOffset _shakeCameraOffset;

        private bool _isShook;
        
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