using UnityEngine;

namespace Core
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _followObject;
        [SerializeField] private Vector3 _cameraOffset;

        [SerializeField] private float _smoothFactor;
        

        private void LateUpdate()
        {
            var newPosition = _followObject.transform.position + _cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPosition, _smoothFactor * Time.deltaTime);
            transform.LookAt(_followObject);
        }
    }
}