using Game.Shooting.Bullets;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Shooting.Cannons
{
    public class TwinsCannon : Cannon
    {
        [Title("Muzzles")]
        [ChildGameObjectsOnly, Required]
        [SerializeField] private Transform _leftMuzzle;
        [ChildGameObjectsOnly, Required]
        [SerializeField] private Transform _rightMuzzle;

        [Title("Parameters")]
        [MinValue(0)]
        [SerializeField] private float _rechargeTime;
        [MinValue(0)]
        [SerializeField] private float _lifetime;
        [MinValue(0)]
        [SerializeField] private int _damage;
        [MinValue(0)]
        [SerializeField] private float _moveSpeed;

        private Bullet.Factory _bulletFactory;

        private Transform _activeMuzzle;
        private float _nextShootTime;

        [Inject]
        public void Construct(Bullet.Factory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

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
            var position = _activeMuzzle.transform.position;
            var rotation = Quaternion.LookRotation(direction);
            var bulletSpecs = new BulletSpecs
            {
                Damage = _damage,
                Direction = direction,
                Lifetime = _lifetime,
                MoveSpeed = _moveSpeed
            };
            
            _bulletFactory.Create(bulletSpecs, position, rotation);
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