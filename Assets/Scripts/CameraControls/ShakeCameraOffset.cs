using UnityEngine;

namespace CameraControls
{
    public class ShakeCameraOffset : MonoBehaviour
    {
        public Vector3 LastValue { get; private set; }
        public Vector3 CurrentValue { get; private set; }
        
        private float _shakeScale;
        public float _dampen = 2f;

        private void Update()
        {
            LastValue = CurrentValue;
            _shakeScale = Mathf.MoveTowards(_shakeScale, 0.0f, _dampen * Time.deltaTime);
            CurrentValue = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized * _shakeScale;
        }

        public void ApplyShakeScale(float shakeScale)
        {
            _shakeScale = shakeScale;
        }
    }
}