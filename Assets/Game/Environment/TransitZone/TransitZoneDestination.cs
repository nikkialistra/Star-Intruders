using UnityEngine;

namespace Game.Environment.TransitZone
{
    [RequireComponent(typeof(BoxCollider))]
    public class TransitZoneDestination : MonoBehaviour
    {
        private BoxCollider _boxCollider;
        private Bounds _bounds;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _bounds = _boxCollider.bounds;
        }

        public Vector3 RandomPointInBox()
        {
            return new Vector3(
                Random.Range(_bounds.min.x, _bounds.max.x),
                Random.Range(_bounds.min.y, _bounds.max.y),
                Random.Range(_bounds.min.z, _bounds.max.z)
                );
        }
    }
}