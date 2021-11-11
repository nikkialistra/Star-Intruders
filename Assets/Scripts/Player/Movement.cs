using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        [Header("Speed")]
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _strafeSpeed;
        [SerializeField] private float _hoverSpeed;
        [SerializeField] private float _lookSpeed;
        [SerializeField] private float _rollSpeed;
        
        [Header("Accelerations")]
        [SerializeField] private float _forwardAcceleration;
        [SerializeField] private float _strafeAcceleration;
        [SerializeField] private float _hoverAcceleration;
        [SerializeField] private float _rollAcceleration;

        private float _currentForwardSpeed;
        private float _currentStrafeSpeed;
        private float _currentHoverSpeed;
        
        private Vector2 _screenCenter;
        private Vector2 _lookOffset;

        private float _currentRoll;
        
        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _screenCenter.x = Screen.width * 0.5f;
            _screenCenter.y = Screen.height * 0.5f;
        }

        public void Move(MoveInput moveInput)
        {
            CalculateLookOffset(moveInput);
            CalculateRoll(moveInput);
            CalculateSpeed(moveInput);

            ApplyRotation();
            ApplySpeed();
        }

        private void CalculateLookOffset(MoveInput moveInput)
        {
            _lookOffset.x = (moveInput.LookPositionValue.x - _screenCenter.x) / _screenCenter.y;
            _lookOffset.y = (moveInput.LookPositionValue.y - _screenCenter.y) / _screenCenter.y;

            _lookOffset = Vector2.ClampMagnitude(_lookOffset, 1f);
        }

        private void CalculateRoll(MoveInput moveInput)
        {
            _currentRoll = Mathf.Lerp(_currentRoll, moveInput.RollValue, _rollAcceleration * Time.fixedDeltaTime);
        }

        private void CalculateSpeed(MoveInput moveInput)
        {
            _currentForwardSpeed = Mathf.Lerp(_currentForwardSpeed, moveInput.ForwardValue * _forwardSpeed,
                _forwardAcceleration * Time.fixedDeltaTime);
            _currentStrafeSpeed = Mathf.Lerp(_currentStrafeSpeed, moveInput.StrafeValue * _strafeSpeed,
                _strafeAcceleration * Time.fixedDeltaTime);
            _currentHoverSpeed = Mathf.Lerp(_currentHoverSpeed, moveInput.HoverValue * _hoverSpeed,
                _hoverAcceleration * Time.fixedDeltaTime);
        }

        private void ApplyRotation()
        {
            var rotation = new Vector3(-_lookOffset.y * _lookSpeed, _lookOffset.x * _lookSpeed, -_currentRoll * _rollSpeed) * Time.fixedDeltaTime;
            _rigidBody.MoveRotation(_rigidBody.rotation * Quaternion.Euler(rotation));
        }

        private void ApplySpeed()
        {
            transform.position += transform.forward * (_currentForwardSpeed * Time.deltaTime);
            transform.position += transform.right * (_currentStrafeSpeed * Time.deltaTime);
            transform.position += transform.up * (_currentHoverSpeed * Time.deltaTime);
        }
    }
}