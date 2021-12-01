using Kernel.Shooting;
using UnityEngine;
using Zenject;

namespace Kernel.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        private Cannon _cannon;

        private Camera _camera;

        [Inject]
        public void Construct(Camera camera, Cannon cannon)
        {
            _camera = camera;
            _cannon = cannon;
        }

        public void Shoot(Vector2 position)
        {
            var direction = _camera.ScreenPointToRay(position).direction;

            _cannon.Shoot(direction);
        }
    }
}