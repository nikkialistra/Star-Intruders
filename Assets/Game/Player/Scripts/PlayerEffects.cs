using Game.Cameras;
using Game.Environment.Asteroids;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [MinValue(0)]
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