using UnityEngine;

namespace UI
{
    public class TargetIcon : MonoBehaviour
    {
        [SerializeField] private Canvas _canvasWithIcon;
        [Space]
        [SerializeField] private Camera _camera;
        

        private bool _hasTarget;

        private Transform _target;

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