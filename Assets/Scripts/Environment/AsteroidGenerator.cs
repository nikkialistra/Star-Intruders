﻿using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Environment
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
        [Space]
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
                asteroid.SetMass();
                asteroid.AddMovement();
                return true;
            }
        }

        private Asteroid InstantiateAsteroid(Vector3 spawnPosition)
        {
            var index = Random.Range(0, _asteroids.Count);
            var scale = Random.Range(_minScale, _maxScale);

            var asteroid = Instantiate(_asteroids[index], spawnPosition, Random.rotation, transform);
            asteroid.transform.localScale = new Vector3(scale, scale, scale);
            return asteroid;
        }

        private bool IsAnotherAsteroidClose(Vector3 spawnPosition, Asteroid asteroid)
        {
            var extents = (asteroid.transform.localScale / 2 ) * _distanceBetweenAsteroidMultiplier;
            var results = new Collider[5];
            var size = Physics.OverlapBoxNonAlloc(spawnPosition, extents, results);

            return size > 0;
        }
    }
}