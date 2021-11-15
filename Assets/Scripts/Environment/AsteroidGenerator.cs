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
        [SerializeField] private float _scaleRange;

        private void Start()
        {
            var quantity = 0;
            while (quantity < _amountToSpawn)
            {
                var spawnPosition = transform.position + new Vector3(Random.insideUnitSphere.x * Random.Range(_minRange, _maxRange), 
                    Random.insideUnitSphere.y * Random.Range(_minRange, _maxRange), Random.Range(_minRange, _maxRange));

                if (spawnPosition.z > transform.position.z)
                {
                    var index = Random.Range(0, _meteors.Count);
                    var scale = Random.Range(1, _scaleRange);

                    var meteor = Instantiate(_meteors[index], spawnPosition, Random.rotation, transform);
                    meteor.transform.localScale = new Vector3(scale, scale, scale);
                    
                    quantity++;
                }
            }
        }
    }
}