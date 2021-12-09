using System;
using System.Collections;
using Game.Intruders.Scripts;
using Kernel.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.IntruderGenerator
{
    [RequireComponent(typeof(BoxCollider))]
    public class IntruderGenerator : MonoBehaviour
    {
        [Title("Spawning")]
        [SerializeField] private bool _infiniteSpawn;
        [EnableIf("@!_infiniteSpawn"), MinValue(0)] 
        [SerializeField] private int _amountToSpawn;

        [Title("Timings")]
        [MinValue(0)]
        [SerializeField] private float _timeBetweenSpawns;
        [MinValue(0)] 
        [SerializeField] private float _timeVariation;

        private Intruder.Factory _intruderFactory;

        private int _spawnedAmount;
        private Bounds _bounds;

        [Inject]
        public void Construct(Intruder.Factory intruderFactory)
        {
            _intruderFactory = intruderFactory;
        }

        private void Awake()
        {
            _bounds = GetComponent<BoxCollider>().bounds;
        }

        private void Start()
        {
            if (_infiniteSpawn)
                _amountToSpawn = Int32.MaxValue;
            
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            while (_spawnedAmount < _amountToSpawn)
            {
                var waitingTime = _timeBetweenSpawns + Random.Range(-_timeVariation, _timeVariation);
                yield return new WaitForSeconds(waitingTime);
                Spawn();
            }
        }

        private void Spawn()
        {
            var spawnPosition = GetRandomPointInBox();
            var specs = GetRandomSpecs();
            Place(spawnPosition, specs);

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
            _intruderFactory.Create(specs, spawnPosition);
        }
    }
}