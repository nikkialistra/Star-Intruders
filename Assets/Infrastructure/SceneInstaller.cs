using Game.Cameras.Scripts;
using Game.Shooting.Bullets;
using Game.Shooting.Cannons.Scripts;
using Game.Shooting.Targeting.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        [Title("Cameras")]
        [Required]
        [SerializeField] private Camera _mainCamera;
        [Required]
        [SerializeField] private ShakeCameraOffset _shakeCameraOffset;
        [Required]
        [SerializeField] private CameraMovement _cameraMovement;

        [Title("Player")]
        [Required]
        [SerializeField] private Transform _chasePoint;
        [Required]
        [SerializeField] private Transform _cockpitPoint;
        
        [Title("Targeting")]
        [Required]
        [SerializeField] private TargetCursor _targetCursor;
        [Required]
        [SerializeField] private TargetIcon _targetIcon;

        [Title("Shooting")]
        [Required]
        [SerializeField] private Cannon _cannon;
        [MinValue(0)] 
        [SerializeField] private int _bulletPoolSize;
        [Required]
        [SerializeField] private GameObject _bulletPrefab;
        [Required] 
        [SerializeField] private Transform _bulletsParent;

        public override void InstallBindings()
        {
            BindCameras();
            
            BindPlayer();
            BindTargeting();
            BindShooting();

            BindFactories();
        }

        private void BindCameras()
        {
            Container.BindInstance(_mainCamera);
            Container.BindInstance(_shakeCameraOffset);
            Container.BindInstance(_cameraMovement);
        }

        private void BindPlayer()
        {
            Container.BindInstance(_chasePoint).WithId("Chase");
            Container.BindInstance(_cockpitPoint).WithId("Cockpit");
        }

        private void BindTargeting()
        {
            Container.BindInstance(_targetCursor);
            Container.BindInstance(_targetIcon);
        }

        private void BindShooting()
        {
            Container.BindInstance(_cannon);
        }

        private void BindFactories()
        {
            Container.BindFactory<BulletSpecs, Bullet, Bullet.Factory>()
                .FromPoolableMemoryPool<BulletSpecs, Bullet, BulletPool>(pool => pool
                .WithInitialSize(_bulletPoolSize)
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransform(_bulletsParent));
        }

        private class BulletPool : MonoPoolableMemoryPool<BulletSpecs, IMemoryPool, Bullet>
        {
        }
    }
}