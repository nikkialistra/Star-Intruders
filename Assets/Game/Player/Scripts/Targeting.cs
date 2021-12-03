using Game.Shooting.Targeting.Scripts;
using Kernel.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Player.Scripts
{
    public class Targeting : MonoBehaviour
    {
        [Required]
        [SerializeField] private TargetCursor _targetCursor;
        [Required]
        [SerializeField] private TargetIcon _targetIcon;
        [Space, MinValue(0)]
        [SerializeField] private float _raycastRadius;
        [MinValue(0)]
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