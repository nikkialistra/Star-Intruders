using UnityEngine;

namespace UI
{
    public class TargetCursor : MonoBehaviour
    {
        [SerializeField] private RectTransform _sprite;

        private Vector2 _screenClampSize;
        private Vector2 _screenCenter;

        private Vector2 _clampedMousePosition;

        private void Start()
        {
            _screenClampSize.x = Screen.width * 0.5f;
            _screenClampSize.y = Screen.height * 0.5f * 0.92f;
            _screenCenter = new Vector2(Screen.width, Screen.height) * 0.5f;
        }

        public void UpdatePosition(Vector2 mousePosition)
        {
            _clampedMousePosition = Vector2.ClampMagnitude(new Vector2(mousePosition.x - Screen.width * 0.5f, mousePosition.y - Screen.height * 0.5f), _screenClampSize.y);
            _sprite.position = _clampedMousePosition + _screenCenter;
        }
    }
}