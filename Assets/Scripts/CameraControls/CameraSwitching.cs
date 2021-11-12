using UnityEngine;

namespace CameraControls
{
    public class CameraSwitching : MonoBehaviour
    {
        public Camera _cockpitCamera;
        public Camera _chaseCamera;
        public Camera _activeCamera;

        public void Switch()
        {
            if (_activeCamera == _chaseCamera)
            {
                SwitchToCockpit();
            }
            else
            {
                SwitchToChase();
            }
        }

        private void SwitchToChase()
        {
            _activeCamera = _chaseCamera;
            _chaseCamera.gameObject.SetActive(true);
            _cockpitCamera.gameObject.SetActive(false);
            _chaseCamera.transform.position = _cockpitCamera.transform.position;
            _chaseCamera.transform.rotation = _cockpitCamera.transform.rotation;

        }

        private void SwitchToCockpit()
        {
            _activeCamera = _cockpitCamera;
            _cockpitCamera.gameObject.SetActive(true);
            _chaseCamera.gameObject.SetActive(false);
        }
    }
}