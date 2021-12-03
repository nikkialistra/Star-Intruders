using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Shooting.Targeting.Scripts
{
    public class TargetIcon : MonoBehaviour
    {
        [Required]
        [SerializeField] private Canvas _canvasWithIcon;
        
        private Camera _camera;

        private bool _hasTarget;

        private Transform _target;

        [Inject]
        public void Construct(Camera camera)
        {
            _camera = camera;
        }

        public void SetTarget(Transform target)
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
                _canvasWithIcon.transform.position = _target.transform.position;
                _canvasWithIcon.transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
            }
        }
    }
}