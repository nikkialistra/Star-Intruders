using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(FlySwitching))]
    [RequireComponent(typeof(Movement))]
    public class KeyboardInput : MonoBehaviour
    {
        private PlayerInput _input;
        
        private FlySwitching _flySwitching;
        private Movement _movement;

        private InputAction _flySwitchAction;
        private InputAction _moveAction;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _flySwitching = GetComponent<FlySwitching>();
            _movement = GetComponent<Movement>();
            
            _flySwitchAction = _input.actions.FindAction("FlySwitch");
            _moveAction = _input.actions.FindAction("Move");
        }

        private void OnEnable()
        {
            _flySwitchAction.started += FlySwitchAction;
        }

        private void OnDisable()
        {
            _flySwitchAction.started -= FlySwitchAction;
        }

        private void Update()
        {
            ReadMoveDirection();
        }

        private void ReadMoveDirection()
        {
            var moveDirection = _moveAction.ReadValue<Vector2>();
            _movement.Move(moveDirection);
        }

        private void FlySwitchAction(InputAction.CallbackContext context)
        {
            _flySwitching.Switch();
        }
    }
}