﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(FlySwitching))]
    [RequireComponent(typeof(Movement))]
    public class KeyboardInput : MonoBehaviour
    {
        private readonly MoveInput _moveInput = new MoveInput();
        
        private PlayerInput _input;
        
        private FlySwitching _flySwitching;
        private Movement _movement;

        private InputAction _forwardAction;
        private InputAction _strafeAction;
        private InputAction _hoverAction;
        private InputAction _rollAction;
        private InputAction _lookPositionAction;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _flySwitching = GetComponent<FlySwitching>();
            _movement = GetComponent<Movement>();
            
            _forwardAction = _input.actions.FindAction("Forward");
            _strafeAction = _input.actions.FindAction("Strafe");
            _hoverAction = _input.actions.FindAction("Hover");
            _rollAction = _input.actions.FindAction("Roll");
            _lookPositionAction = _input.actions.FindAction("LookPosition");
        }

        private void Update()
        {
            ReadMovementActions();
        }

        private void ReadMovementActions()
        {
            _moveInput.ForwardValue = _forwardAction.ReadValue<float>();
            _moveInput.StrafeValue = _strafeAction.ReadValue<float>();
            _moveInput.HoverValue = _hoverAction.ReadValue<float>();
            _moveInput.RollValue = _rollAction.ReadValue<float>();
            _moveInput.LookPositionValue = _lookPositionAction.ReadValue<Vector2>();
            
            _movement.Move(_moveInput);
        }
    }
}