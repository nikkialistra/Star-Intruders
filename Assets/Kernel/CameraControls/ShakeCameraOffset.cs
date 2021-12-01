using UnityEngine;

namespace Kernel.CameraControls
{
    public class ShakeCameraOffset : MonoBehaviour
    {
        public Vector3 LastValue { get; private set; }
        public Vector3 CurrentValue { get; private set; }
        
        private float _shake;
        public float _dampen = 2f;

        private void Update()
        {
            LastValue = CurrentValue;
            _shake = Mathf.MoveTowards(_shake, 0.0f, _dampen * Time.deltaTime);
            CurrentValue = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized * _shake;
        }

        public void ApplyShake(float shakeScale)
        {
            _shake = shakeScale;
        }
    }
}