using Entities.Shooting;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeReference] private Cannon _cannon;

        [SerializeField] private Camera _camera;

        public void Shoot(Vector2 position)
        {
            var direction = _camera.ScreenPointToRay(position).direction;

            _cannon.Shoot(direction);
        }
    }
}