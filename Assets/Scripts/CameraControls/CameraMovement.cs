using UnityEngine;

namespace CameraControls
{
    [RequireComponent(typeof(ShakeCameraOffset))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        
        [SerializeField] private Transform _chasePoint;
        [SerializeField] private Transform _cockpitPoint;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private float _switchToCockpitMoveSpeed;
        [SerializeField] private float _switchToCockpitRotateSpeed;

        private bool _isChasing = true;
        
        private bool _switchingFinished;

        private ShakeCameraOffset _shakeCameraOffset;

        private void Awake()
        {
            _shakeCameraOffset = GetComponent<ShakeCameraOffset>();
        }

        private void FixedUpdate()
        {
            SubtractLastShake();
            MoveCamera();
            AddNewShake();
        }

        private void MoveCamera()
        {
            if (_isChasing)
            {
                MoveByChasing();
            }
            else
            {
                MoveFromCockpit();
            }
        }

        private void MoveByChasing()
        {
            _cameraTransform.position =
                Vector3.Lerp(_cameraTransform.position, _chasePoint.position, _moveSpeed * Time.deltaTime);
            _cameraTransform.rotation =
                Quaternion.Lerp(_cameraTransform.rotation, _chasePoint.rotation, _rotateSpeed * Time.deltaTime);
        }

        private void MoveFromCockpit()
        {
            if (_switchingFinished)
            {
                _cameraTransform.position = _cockpitPoint.position;
                _cameraTransform.rotation = _cockpitPoint.rotation;
            }
            else
            {
                _cameraTransform.position =
                    Vector3.Lerp(_cameraTransform.position, _cockpitPoint.position, _switchToCockpitMoveSpeed * Time.deltaTime);
                _cameraTransform.rotation =
                    Quaternion.Lerp(_cameraTransform.rotation, _cockpitPoint.rotation,
                        _switchToCockpitRotateSpeed * Time.deltaTime);
            }

            if ((_cameraTransform.position - _cockpitPoint.position).magnitude < 0.3f)
            {
                _switchingFinished = true;
            }
        }

        private void SubtractLastShake()
        {
            transform.position -= _shakeCameraOffset.LastValue;
        }

        private void AddNewShake()
        {
            transform.position += _shakeCameraOffset.CurrentValue;
        }

        public void SwitchView()
        {
            _isChasing = !_isChasing;
            _switchingFinished = false;
        }
    }
}