using Game.Cameras;
using Kernel.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(FlySwitching))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(Targeting))]
    [RequireComponent(typeof(PlayerShooting))]
    public class KeyboardInput : MonoBehaviour
    {
        private CameraMovement _cameraMovement;

        private readonly MoveInput _moveInput = new();
        
        private PlayerInput _input;
        
        private FlySwitching _flySwitching;
        private PlayerMovement _movement;
        private Targeting _targeting;
        private PlayerShooting _shooting;

        private InputAction _forwardAction;
        private InputAction _strafeAction;
        private InputAction _hoverAction;
        private InputAction _rollAction;
        private InputAction _lookPositionAction;
        private InputAction _accelerationAction;
        private InputAction _switchViewAction;
        private InputAction _shootAction;

        [Inject]
        public void Construct(CameraMovement cameraMovement)
        {
            _cameraMovement = cameraMovement;
        }

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _flySwitching = GetComponent<FlySwitching>();
            _movement = GetComponent<PlayerMovement>();
            _targeting = GetComponent<Targeting>();
            _shooting = GetComponent<PlayerShooting>();
            
            _forwardAction = _input.actions.FindAction("Forward");
            _strafeAction = _input.actions.FindAction("Strafe");
            _hoverAction = _input.actions.FindAction("Hover");
            _lookPositionAction = _input.actions.FindAction("LookPosition");
            _rollAction = _input.actions.FindAction("Roll");
            _accelerationAction = _input.actions.FindAction("Acceleration");
            _switchViewAction = _input.actions.FindAction("SwitchView");
            _shootAction = _input.actions.FindAction("Shoot");
        }

        private void Update()
        {
            ReadMovementActions();
            UpdateTargetCursor();
            ReadShootingActions();
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
            _targeting.UpdateFromInput(_lookPositionAction.ReadValue<Vector2>());
        }

        private void Move()
        {
            _movement.Move(_moveInput);
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

        private void ReadShootingActions()
        {
            if (_shootAction.ReadValue<float>() > 0)
            {
                _shooting.Shoot(_lookPositionAction.ReadValue<Vector2>());
            }
        }
    }
}