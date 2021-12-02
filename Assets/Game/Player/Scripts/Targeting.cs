using Game.Shooting.Targeting.Scripts;
using Kernel.Types;
using UnityEngine;
using Zenject;

namespace Game.Player.Scripts
{
    public class Targeting : MonoBehaviour
    {
        [SerializeField] private TargetCursor _targetCursor;
        [SerializeField] private TargetIcon _targetIcon;
        [Space]
        [SerializeField] private float _raycastRadius;
        [SerializeField] private float _maxDistanceToTarget;
        
        private Camera _camera;

        [Inject]
        public void Construct(Camera camera)
        {
            _camera = camera;
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
                    _targetIcon.SetTarget(targetable.transform);
                }
            }
        }
    }
}