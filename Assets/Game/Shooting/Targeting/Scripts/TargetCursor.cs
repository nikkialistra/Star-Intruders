using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Shooting.Targeting.Scripts
{
    public class TargetCursor : MonoBehaviour
    {
        public Vector2 Position => _sprite.position;
        
        [Required]
        [SerializeField] private RectTransform _sprite;
        
        private Vector2 _screenClampSize;
        private Vector2 _screenCenter;

        private void Start()
        {
            _screenClampSize.x = Screen.width * 0.5f;
            _screenClampSize.y = Screen.height * 0.5f * 0.92f;
            _screenCenter = new Vector2(Screen.width, Screen.height) * 0.5f;
        }

        public void SetPosition(Vector2 mousePosition)
        {
            var clampedMousePosition = Vector2.ClampMagnitude(new Vector2(mousePosition.x - Screen.width * 0.5f, mousePosition.y - Screen.height * 0.5f), _screenClampSize.y);
            _sprite.position = clampedMousePosition + _screenCenter;
        }
    }
}