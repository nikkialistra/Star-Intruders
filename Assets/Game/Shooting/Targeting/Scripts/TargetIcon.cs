using Kernel.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Shooting.Targeting
{
    public class TargetIcon : MonoBehaviour
    {
        [Required]
        [SerializeField] private Canvas _canvasWithIcon;
        
        private Camera _camera;

        private bool _hasTarget;

        private Targetable _target;

        [Inject]
        public void Construct(Camera camera)
        {
            _camera = camera;
        }

        public void SetTarget(Targetable target)
        {
            _target = target;
            _hasTarget = true;
        }
    
        public void UnsetTarget()
        {
            _hasTarget = false;
        }

        private void Update()
        {
            if (_hasTarget)
            {
                _canvasWithIcon.transform.position = _target.Position;
                _canvasWithIcon.transform.localScale = _target.Scale;
                
                _canvasWithIcon.transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                    _camera.transform.rotation * Vector3.up);
            }
        }
    }
}