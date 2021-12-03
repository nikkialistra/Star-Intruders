using Kernel.Controls;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FlySwitching))]
    [RequireComponent(typeof(PlayerAnimations))]
    public class PlayerMovement : MonoBehaviour
    {
        [Title("Speed"), MinValue(0)]
        [SerializeField] private float _forwardSpeed;
        [MinValue(0)]
        [SerializeField] private float _strafeSpeed;
        [MinValue(0)]
        [SerializeField] private float _hoverSpeed;
        [MinValue(0)]
        [SerializeField] private float _lookSpeed;
        [MinValue(0)]
        [SerializeField] private float _rollSpeed;
        
        [Title("Accelerations"), MinValue(0)]
        [SerializeField] private float _forwardAcceleration;
        [MinValue(0)]
        [SerializeField] private float _strafeAcceleration;
        [MinValue(0)]
        [SerializeField] private float _hoverAcceleration;
        [MinValue(0)]
        [SerializeField] private float _rollAcceleration;
        [Space, MinValue(0)]
        [SerializeField] private float _accelerationMultiplier;

        private float _currentForwardSpeed;
        private float _currentStrafeSpeed;
        private float _currentHoverSpeed;

        private Vector2 _screenCenter;
        private Vector2 _lookOffset;

        private float _currentRoll;

        private Rigidbody _rigidBody;
        private FlySwitching _flySwitching;
        private PlayerAnimations _playerAnimations;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _flySwitching = GetComponent<FlySwitching>();
            _playerAnimations = GetComponent<PlayerAnimations>();
        }

        private void Start()
        {
            _screenCenter.x = Screen.width * 0.5f;
            _screenCenter.y = Screen.height * 0.5f;
        }

        public void Move(MoveInput moveInput)
        {
            if (!_flySwitching.CanFly)
            {
                return;
            }

            CalculateSpeed(moveInput);
            CalculateLookOffset(moveInput);
            CalculateRoll(moveInput);

            ApplySpeed();
            ApplyRotation();

            UpdateMoveAnimations(moveInput);
        }

        private void CalculateSpeed(MoveInput moveInput)
        {
            var acceleration = moveInput.Accelerated ? _accelerationMultiplier : 1;

            _currentForwardSpeed = Mathf.Lerp(_currentForwardSpeed, moveInput.ForwardValue * _forwardSpeed * acceleration,
                _forwardAcceleration * Time.fixedDeltaTime);
            _currentStrafeSpeed = Mathf.Lerp(_currentStrafeSpeed, moveInput.StrafeValue * _strafeSpeed,
                _strafeAcceleration * Time.fixedDeltaTime);
            _currentHoverSpeed = Mathf.Lerp(_currentHoverSpeed, moveInput.HoverValue * _hoverSpeed,
                _hoverAcceleration * Time.fixedDeltaTime);
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

        private void ApplySpeed()
        {
            var velocity = (transform.forward * _currentForwardSpeed) + (transform.right * _currentStrafeSpeed) +
                           (transform.up * _currentHoverSpeed);
            _rigidBody.velocity = velocity;
        }

        private void ApplyRotation()
        {
            var rotation = new Vector3(-_lookOffset.y * _lookSpeed, _lookOffset.x * _lookSpeed, -_currentRoll * _rollSpeed) * Time.fixedDeltaTime;
            _rigidBody.MoveRotation(_rigidBody.rotation * Quaternion.Euler(rotation));
        }

        private void UpdateMoveAnimations(MoveInput moveInput)
        {
            if (moveInput.ForwardValue > 0)
            {
                _playerAnimations.Move();
            }
            else
            {
                _playerAnimations.Stop();
            }
        }
    }
}