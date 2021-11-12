using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class MeteorGenerator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _meteors;

        [SerializeField] private int _quantityToGenerate;
        [SerializeField] private float _radius;
        [SerializeField] private float _scaleRange;

        private void Start()
        {
            var quantity = 0;
            while (quantity < _quantityToGenerate)
            {
                var spawnPosition = new Vector3(Random.insideUnitSphere.x * _radius, 
                    Random.insideUnitSphere.y * _radius, Random.insideUnitSphere.z * _radius);

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