using UnityEngine;

namespace Entities.Shooting
{
    public class TwinsCannon : MonoBehaviour, ICannon
    {
        [Header("Muzzles")]
        [SerializeField] private Transform _firstMuzzle;
        [SerializeField] private Transform _secondMuzzle;
        
        [Header("Bullet")]
        [SerializeField] private Bullet _bullet;
        
        [Header("Parameters")]
        [SerializeField] private float _rechargeTime;
        [SerializeField] private float _lifetime;
        [SerializeField] private float _damage;
        [SerializeField] private float _moveSpeed;

        private Transform _activeMuzzle;
        private float _nextShootTime;

        private void Start()
        {
            _activeMuzzle = _firstMuzzle;
        }

        public void Shoot(Vector3 direction)
        {
            if (IsRechargeFinished())
            {
                Instantiate(_bullet, _activeMuzzle.transform.position, Quaternion.identity);
                _bullet.Initialize(_lifetime, _damage, _moveSpeed, direction);
                SwitchMuzzle();
            }
        }

        private bool IsRechargeFinished()
        {
            if (Time.time > _nextShootTime)
            {
                _nextShootTime += _rechargeTime;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SwitchMuzzle()
        {
            if (_activeMuzzle == _firstMuzzle)
            {
                _activeMuzzle = _secondMuzzle;
            }
            else
            {
                _activeMuzzle = _firstMuzzle;
            }
        }
    }
}