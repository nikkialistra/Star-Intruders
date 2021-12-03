﻿using Game.Shooting.Bullets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Shooting.Cannons.Scripts
{
    public class TwinsCannon : Cannon
    {
        [Title("Muzzles"), ChildGameObjectsOnly, Required] 
        [SerializeField] private Transform _leftMuzzle;
        [ChildGameObjectsOnly, Required]
        [SerializeField] private Transform _rightMuzzle;

        [Title("Bullet"), Required] 
        [SerializeField] private Bullet _bullet;
        [ChildGameObjectsOnly, Required]
        [SerializeField] private Transform _bulletsParent;

        [Title("Parameters"), MinValue(0)] 
        [SerializeField]
        private float _rechargeTime;

        [MinValue(0)]
        [SerializeField] private float _lifetime;
        [MinValue(0)]
        [SerializeField] private float _damage;
        [MinValue(0)]
        [SerializeField] private float _moveSpeed;

        private Transform _activeMuzzle;
        private float _nextShootTime;

        private void Start()
        {
            _activeMuzzle = _leftMuzzle;
        }

        public override Vector3 GetMuzzlePosition()
        {
            return _activeMuzzle.transform.position;
        }

        public override void Shoot(Vector3 direction)
        {
            if (IsRechargeFinished())
            {
                CalculateNextRecharge();
                CreateBullet(direction);
                SwitchMuzzle();
            }
        }

        private void CreateBullet(Vector3 direction)
        {
            var bullet = Instantiate(_bullet, _activeMuzzle.transform.position, Quaternion.LookRotation(direction),
                _bulletsParent);
            bullet.Initialize(_lifetime, _damage, _moveSpeed, direction);
        }

        private bool IsRechargeFinished()
        {
            return Time.time > _nextShootTime;
        }

        private void CalculateNextRecharge()
        {
            _nextShootTime = Time.time + _rechargeTime;
        }

        private void SwitchMuzzle()
        {
            if (_activeMuzzle == _leftMuzzle)
            {
                _activeMuzzle = _rightMuzzle;
            }
            else
            {
                _activeMuzzle = _leftMuzzle;
            }
        }
    }
}