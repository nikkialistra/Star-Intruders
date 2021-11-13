using UnityEngine;

namespace CameraControls
{
    [RequireComponent(typeof(ShakeCameraOffset))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _targetPoint;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        
        [SerializeField] private bool _shouldFollow;

        private ShakeCameraOffset _shakeCameraOffset;

        private void Awake()
        {
            _shakeCameraOffset = GetComponent<ShakeCameraOffset>();
        }

        private void FixedUpdate()
        {
            transform.position -= _shakeCameraOffset.LastValue;

            if (_shouldFollow)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPoint.position, _moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, _targetPoint.rotation, _rotateSpeed * Time.deltaTime);
            }

            transform.position += _shakeCameraOffset.CurrentValue;
        }

        public void ResetCamera()
        {
            _shouldFollow = true;
            transform.position = _targetPoint.position;
            transform.rotation = _targetPoint.rotation;
        }
    }
}