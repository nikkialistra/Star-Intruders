using Entities.Types;
using UI;
using UnityEngine;

namespace Entities.Player
{
    public class Targeting : MonoBehaviour
    {
        [SerializeField] private TargetCursor _targetCursor;
        [SerializeField] private TargetIcon _targetIcon;
        [Space]
        [SerializeField] private Camera _camera;
        [Space]
        [SerializeField] private float _maxDistanceToTarget;

        public void UpdateFromInput(Vector2 mousePosition)
        {
            _targetCursor.SetPosition(mousePosition);
            SearchForTarget(_targetCursor.Position);
        }

        private void SearchForTarget(Vector2 viewportPosition)
        {
            var ray = _camera.ScreenPointToRay(viewportPosition);
            if (Physics.Raycast (ray, out var hit, _maxDistanceToTarget)) {
                if (hit.transform.TryGetComponent(out Targetable targetable))
                {
                    _targetIcon.SetTarget(targetable.transform);
                }
            }
        }
    }
}