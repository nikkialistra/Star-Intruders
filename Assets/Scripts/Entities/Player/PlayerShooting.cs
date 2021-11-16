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
            var rayStart = _cannon.GetMuzzlePosition();
            var rayEnd = _camera.ScreenToWorldPoint(position);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = rayEnd;
            var direction = (rayStart - rayEnd).normalized;
            Debug.DrawRay(rayStart, direction * 2000, Color.red, 1.5f);
            
            _cannon.Shoot(direction);
        }
    }
}