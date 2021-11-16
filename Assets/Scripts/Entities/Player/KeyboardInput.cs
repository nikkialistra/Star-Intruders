using Core.CameraControls;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(FlySwitching))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(CameraMovement))]
    public class KeyboardInput : MonoBehaviour
    {
        [SerializeField] private TargetCursor _targetCursor;

        private readonly MoveInput _moveInput = new MoveInput();
        
        private PlayerInput _input;
        
        private FlySwitching _flySwitching;
        private PlayerMovement _playerMovement;
        private CameraMovement _cameraMovement;

        private InputAction _forwardAction;
        private InputAction _strafeAction;
        private InputAction _hoverAction;
        private InputAction _rollAction;
        private InputAction _lookPositionAction;
        private InputAction _accelerationAction;
        private InputAction _switchViewAction;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _flySwitching = GetComponent<FlySwitching>();
            _playerMovement = GetComponent<PlayerMovement>();
            _cameraMovement = GetComponent<CameraMovement>();
            
            _forwardAction = _input.actions.FindAction("Forward");
            _strafeAction = _input.actions.FindAction("Strafe");
            _hoverAction = _input.actions.FindAction("Hover");
            _lookPositionAction = _input.actions.FindAction("LookPosition");
            _rollAction = _input.actions.FindAction("Roll");
            _accelerationAction = _input.actions.FindAction("Acceleration");
            _switchViewAction = _input.actions.FindAction("SwitchView");
        }

        private void Update()
        {
            ReadMovementActions();
            UpdateTargetCursor();
        }

        private void FixedUpdate()
        {
            Move();
            TryFlySwitch();
        }

        private void OnEnable()
        {
            _switchViewAction.started += SwitchView;
        }

        private void OnDisable()
        {
            _switchViewAction.started -= SwitchView;
        }

        private void ReadMovementActions()
        {
            _moveInput.ForwardValue = _forwardAction.ReadValue<float>();
            _moveInput.StrafeValue = _strafeAction.ReadValue<float>();
            _moveInput.HoverValue = _hoverAction.ReadValue<float>();
            _moveInput.RollValue = _rollAction.ReadValue<float>();
            _moveInput.LookPositionValue = _lookPositionAction.ReadValue<Vector2>();
            _moveInput.Accelerated = _accelerationAction.ReadValue<float>() > 0;
        }

        private void UpdateTargetCursor()
        {
            _targetCursor.UpdatePosition(_lookPositionAction.ReadValue<Vector2>());
        }

        private void Move()
        {
            _playerMovement.Move(_moveInput);
        }

        private void TryFlySwitch()
        {
            if (_moveInput.HoverValue < 0f)
            {
                _flySwitching.TryLand();
            } else if (_moveInput.HoverValue > 0f)
            {
                _flySwitching.TryTakeoff();
            }
        }

        private void SwitchView(InputAction.CallbackContext context)
        {
            _cameraMovement.SwitchView();
        }
    }
}