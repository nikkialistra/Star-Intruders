using Game.Shooting.Cannons;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform _cannonPoint;
        
        private Cannon _cannon;

        private Camera _camera;

        [Inject]
        public void Construct(Camera camera, Cannon cannon)
        {
            _camera = camera;
            _cannon = cannon;
            _cannon.transform.SetParent(_cannonPoint, false);
        }

        public void Shoot(Vector2 position)
        {
            var direction = _camera.ScreenPointToRay(position).direction;

            _cannon.Shoot(direction);
        }
    }
}