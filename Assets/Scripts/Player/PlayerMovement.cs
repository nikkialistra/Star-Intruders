using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FlySwitching))]
    [RequireComponent(typeof(PlayerAnimations))]
    public class PlayerMovement : MonoBehaviour
    {
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
        [Space]
        [SerializeField] private float _accelerationMultiplier;
        [Space]
        [SerializeField] private float _moveThreshold;

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

            UpdateMoveAnimations();
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

        private void UpdateMoveAnimations()
        {
            if (_rigidBody.velocity.magnitude > _moveThreshold)
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