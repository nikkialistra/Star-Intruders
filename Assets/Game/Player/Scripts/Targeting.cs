using Game.Shooting.Targeting;
using Kernel.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class Targeting : MonoBehaviour
    {
        [MinValue(0)]
        [SerializeField] private float _raycastRadius;
        [MinValue(0)]
        [SerializeField] private float _maxDistanceToTarget;
        
        private TargetCursor _targetCursor;
        private TargetIcon _targetIcon;
        
        private Camera _camera;

        [Inject]
        public void Construct(Camera camera, TargetCursor targetCursor, TargetIcon targetIcon)
        {
            _camera = camera;
            _targetCursor = targetCursor;
            _targetIcon = targetIcon;
        }

        public void UpdateFromInput(Vector2 mousePosition)
        {
            _targetCursor.SetPosition(mousePosition);
            SearchForTarget(_targetCursor.Position);
        }

        private void SearchForTarget(Vector2 viewportPosition)
        {
            var ray = _camera.ScreenPointToRay(viewportPosition);
            if (Physics.SphereCast(ray, _raycastRadius, out var hit, _maxDistanceToTarget)) {
                if (hit.transform.TryGetComponent(out Targetable targetable))
                {
                    _targetIcon.SetTarget(targetable);
                }
            }
        }
    }
}