using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class AsteroidGenerator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _meteors;

        [SerializeField] private int _amountToSpawn;
        [SerializeField] private float _minRange;
        [SerializeField] private float _maxRange;
        [SerializeField] private float _maxScale;
        [SerializeField] private float _distanceBetweenAsteroidMultiplier;

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            var quantity = 0;
            while (quantity < _amountToSpawn)
            {
                var spawnPosition = GetSpawnPosition();

                if (IsPositionBeyondBase(spawnPosition))
                {
                    if (TryPlace(spawnPosition))
                    {
                        quantity++;
                    }
                }
            }
        }

        private Vector3 GetSpawnPosition()
        {
            var spawnPosition = transform.position + new Vector3(
                Random.insideUnitSphere.x * Random.Range(_minRange, _maxRange),
                Random.insideUnitSphere.y * Random.Range(_minRange, _maxRange), Random.Range(_minRange, _maxRange));
            return spawnPosition;
        }

        private bool IsPositionBeyondBase(Vector3 spawnPosition)
        {
            return spawnPosition.z > transform.position.z;
        }

        private bool TryPlace(Vector3 spawnPosition)
        {
            var asteroid = InstantiateAsteroid(spawnPosition);

            if (IsAnotherAsteroidClose(spawnPosition, asteroid))
            {
                Destroy(asteroid);
                return false;
            }
            else
            {
                return true;
            }
        }

        private GameObject InstantiateAsteroid(Vector3 spawnPosition)
        {
            var index = Random.Range(0, _meteors.Count);
            var scale = Random.Range(1, _maxScale);

            var asteroid = Instantiate(_meteors[index], spawnPosition, Random.rotation, transform);
            asteroid.transform.localScale = new Vector3(scale, scale, scale);
            return asteroid;
        }

        private bool IsAnotherAsteroidClose(Vector3 spawnPosition, GameObject asteroid)
        {
            var boxBounds = new Bounds(spawnPosition, asteroid.GetComponent<MeshCollider>().bounds.size);
            var sqrHalfBoxSize = boxBounds.extents.sqrMagnitude;
            var overlappingSphereRadius = Mathf.Sqrt(sqrHalfBoxSize + sqrHalfBoxSize);
            var checkedDistance = overlappingSphereRadius * _distanceBetweenAsteroidMultiplier;

            var results = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(spawnPosition, checkedDistance, results);

            return size > 1;
        }
    }
}