using System.Collections.Generic;
using Game.Environment.Asteroids;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Environment.AsteroidGenerator
{
    public class AsteroidGenerator : MonoBehaviour
    {
        [ValidateInput("@$value.Count > 0"), AssetsOnly]
        [SerializeField] private List<Asteroid> _asteroids;

        [MinValue(0)]
        [SerializeField] private int _amountToSpawn;
        [Space]
        [MinValue(0)]
        [SerializeField] private float _minRange;
        [MinValue(0)]
        [SerializeField] private float _maxRange;
        [Space]
        [MinValue(0)]
        [SerializeField] private float _minScale;
        [MinValue(0)]
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
            
            var scale = Random.Range(_minScale, _maxScale);
            asteroid.SetScale(scale);
            
            asteroid.AddMovement();
        }

        private Asteroid InstantiateAsteroid(Vector3 spawnPosition)
        {
            var index = Random.Range(0, _asteroids.Count);
            var asteroid = Instantiate(_asteroids[index], spawnPosition, Random.rotation, transform);
            
            return asteroid;
        }
    }
}