﻿using UnityEngine;
using Zenject;

namespace Game.Cameras.Scripts
{
    [RequireComponent(typeof(ShakeCameraOffset))]
    public class CameraMovement : MonoBehaviour
    {
        private Transform _chasePoint;
        private Transform _cockpitPoint;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private Camera _overlayCamera;

        private bool _isChasing = true;
        
        private bool _switchingFinished;

        private ShakeCameraOffset _shakeCameraOffset;

        [Inject]
        public void Construct([Inject(Id = "Chase")] Transform chasePoint, [Inject(Id = "Cockpit")] Transform cockpitPoint)
        {
            _chasePoint = chasePoint;
            _cockpitPoint = cockpitPoint;
        }

        private void Awake()
        {
            _shakeCameraOffset = GetComponent<ShakeCameraOffset>();
        }

        private void FixedUpdate()
        {
            SubtractLastShake();
            MoveCamera();
            AddNewShake();
            UpdateOverlayCameraTransform();
        }

        public void SwitchView()
        {
            _isChasing = !_isChasing;
        }

        private void MoveCamera()
        {
            if (_isChasing)
            {
                MoveToChasePoint();
            }
            else
            {
                MoveToCockpitPoint();
            }
        }

        private void MoveToChasePoint()
        {
            transform.position =
                Vector3.Lerp(transform.position, _chasePoint.position, _moveSpeed * Time.deltaTime);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, _chasePoint.rotation, _rotateSpeed * Time.deltaTime);
        }

        private void MoveToCockpitPoint()
        {
            transform.position = _cockpitPoint.position;
            transform.rotation = _cockpitPoint.rotation;

        }

        private void SubtractLastShake()
        {
            transform.position -= _shakeCameraOffset.LastValue;
        }

        private void AddNewShake()
        {
            transform.position += _shakeCameraOffset.CurrentValue;
        }

        private void UpdateOverlayCameraTransform()
        {
            _overlayCamera.transform.position = transform.position;
            _overlayCamera.transform.rotation = transform.rotation;
        }
    }
}