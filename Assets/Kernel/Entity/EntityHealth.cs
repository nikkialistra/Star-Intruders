using System;
using System.Collections;
using Kernel.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kernel.Entity
{
    public class EntityHealth : MonoBehaviour, IDamageable
    {
        [MinValue(0)]
        [SerializeField] private int _fullHealth;
        [ValidateInput("@_startHealth <= _fullHealth", "Start health cannon be greater than full health"), MinValue(0)]
        [SerializeField] private int _startHealth;

        public event Action Died;
        public event Action<int> HealthChange; 
        
        public int Health
        {
            get => _health;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Health));
                }

                _health = value;
            }
        }

        private int _health;
        private bool IsAlive => _health > 0;
        
        private Coroutine _takingDamage;
        
        public void TakeDamage(int value)
        {
            CheckTakeDamageValidity(value);

            _health -= value;
            HealthChange?.Invoke(_health);

            if (!IsAlive)
            {
                StopTakingDamage();
                Died?.Invoke();
            }
        }

        public void TakeDamageContinuously(int value, float interval, float time)
        {
            if (_takingDamage != null)
            {
                StopCoroutine(_takingDamage);
            }

            _takingDamage = StartCoroutine(TakingDamage(value, interval, time));
        }

        public void StopTakingDamage()
        {
            if (_takingDamage != null)
            {
                StopCoroutine(_takingDamage);
            }
        }
        
        private IEnumerator TakingDamage(int value, float interval, float time)
        {
            var elapsedTime = 0.0f;
            
            while (elapsedTime < time)
            {
                TakeDamage(value);
                yield return new WaitForSeconds(interval);
                elapsedTime += interval;
            }
        }

        private void CheckTakeDamageValidity(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Damage must be more than zero");
            }

            if (!IsAlive)
            {
                throw new InvalidOperationException("Damage cannon be applied to died entity");
            }
        }
    }
}