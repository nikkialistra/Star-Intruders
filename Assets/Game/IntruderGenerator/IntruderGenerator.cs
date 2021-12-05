using System.Collections.Generic;
using Game.Intruders.Scripts;
using Kernel.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.IntruderGenerator
{
    [RequireComponent(typeof(BoxCollider))]
    public class IntruderGenerator : MonoBehaviour
    {
        [ValidateInput("@$value.Count > 0"), AssetsOnly]
        [SerializeField] private List<Intruder> _intruders;

        [MinValue(0)] 
        [SerializeField] private int _amountToSpawn;

        private Bounds _bounds;

        private void Awake()
        {
            _bounds = GetComponent<BoxCollider>().bounds;
        }

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            for (int i = 0; i < _amountToSpawn; i++)
            {
                var spawnPosition = GetRandomPointInBox();
                var specs = GetRandomSpecs();
                Place(spawnPosition, specs);
            }
        }

        private Vector3 GetRandomPointInBox()
        {
            return new Vector3(
                Random.Range(_bounds.min.x, _bounds.max.x),
                Random.Range(_bounds.min.y, _bounds.max.y),
                Random.Range(_bounds.min.z, _bounds.max.z)
            );
        }

        private IntruderSpecs GetRandomSpecs()
        {
            var color = EnumUtilities.RandomEnumValue<IntruderColor>();
            var flag = EnumUtilities.RandomEnumValue<IntruderFlag>();

            var specs = new IntruderSpecs()
            {
                Color = color,
                Flag = flag
            };
            return specs;
        }

        private void Place(Vector3 spawnPosition, IntruderSpecs specs)
        {
            var index = Random.Range(0, _intruders.Count);

            var intruder = Instantiate(_intruders[index], spawnPosition, Quaternion.identity, transform);
            intruder.Initialize(specs);
        }
    }
}