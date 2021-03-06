using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Cameras
{
    public class ShakeCameraOffset : MonoBehaviour
    {
        public Vector3 LastValue { get; private set; }
        public Vector3 CurrentValue { get; private set; }
        
        [MinValue(0)]
        [SerializeField] private float _dampen = 2f;
        
        private float _shake;

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