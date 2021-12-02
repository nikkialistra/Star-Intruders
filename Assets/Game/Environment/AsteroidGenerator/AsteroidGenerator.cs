using System.Collections.Generic;
using Game.Environment.Asteroids;
using UnityEngine;

namespace Game.Environment.AsteroidGenerator
{
    public class AsteroidGenerator : MonoBehaviour
    {
        [SerializeField] private List<Asteroid> _asteroids;

        [SerializeField] private int _amountToSpawn;
        [Space]
        [SerializeField] private float _minRange;
        [SerializeField] private float _maxRange;
        [Space]
        [SerializeField] private float _minScale;
        [SerializeField] private float _maxScale;

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
                    Place(spawnPosition);
                    quantity++;
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

        private void Place(Vector3 spawnPosition)
        {
            var asteroid = InstantiateAsteroid(spawnPosition);
            asteroid.SetMass();
            asteroid.AddMovement();
        }

        private Asteroid InstantiateAsteroid(Vector3 spawnPosition)
        {
            var index = Random.Range(0, _asteroids.Count);
            var scale = Random.Range(_minScale, _maxScale);

            var asteroid = Instantiate(_asteroids[index], spawnPosition, Random.rotation, transform);
            asteroid.transform.localScale = new Vector3(scale, scale, scale);
            return asteroid;
        }
    }
}