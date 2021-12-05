using Sirenix.OdinInspector;
using UnityEngine;

namespace Kernel.Types
{
    public class Targetable : MonoBehaviour
    { 
        public Vector3 Position => transform.position;
        public Vector3 Scale
        {
            get
            {
                var scaleOnAxis = transform.localScale.x * _indicatorScale;
                return new Vector3(scaleOnAxis, scaleOnAxis, scaleOnAxis);
            }
        }
        
        [MinValue(0)]
        [SerializeField] private float _indicatorScale = 1;
    }
}