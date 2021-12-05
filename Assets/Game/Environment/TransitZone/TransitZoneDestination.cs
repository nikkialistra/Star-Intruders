using UnityEngine;

namespace Game.Environment.TransitZone
{
    [RequireComponent(typeof(BoxCollider))]
    public class TransitZoneDestination : MonoBehaviour
    {
        private Bounds _bounds;

        private void Awake()
        {
            _bounds = GetComponent<BoxCollider>().bounds;
        }

        public Vector3 GetRandomPointInBox()
        {
            return new Vector3(
                Random.Range(_bounds.min.x, _bounds.max.x),
                Random.Range(_bounds.min.y, _bounds.max.y),
                Random.Range(_bounds.min.z, _bounds.max.z)
                );
        }
    }
}