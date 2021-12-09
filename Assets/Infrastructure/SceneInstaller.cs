using Game.Cameras.Scripts;
using Game.Environment.TransitZone;
using Game.Intruders.Scripts;
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
        
        [Title("Intruders")]
        [MinValue(0)] 
        [SerializeField] private int _intruderPoolSize;
        [Required]
        [SerializeField] private GameObject _intruderPrefab;
        [Required] 
        [SerializeField] private Transform _intrudersParent;
        
        [Title("TransitZone")]
        [Required]
        [SerializeField] private TransitZoneDestination _transitZoneDestination;

        public override void InstallBindings()
        {
            BindCameras();
            
            BindPlayer();
            BindTargeting();
            BindShooting();
            BindTransitZone();

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

        private void BindTransitZone()
        {
            Container.BindInstance(_transitZoneDestination);
        }

        private void BindFactories()
        {
            Container.BindFactory<BulletSpecs, Vector3, Quaternion, Bullet, Bullet.Factory>()
                .FromPoolableMemoryPool<BulletSpecs, Vector3, Quaternion, Bullet, BulletPool>(pool => pool
                .WithInitialSize(_bulletPoolSize)
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransform(_bulletsParent));
            
            Container.BindFactory<IntruderSpecs, Vector3, Intruder, Intruder.Factory>()
                .FromPoolableMemoryPool<IntruderSpecs, Vector3, Intruder, IntruderPool>(pool => pool
                    .WithInitialSize(_intruderPoolSize)
                    .FromComponentInNewPrefab(_intruderPrefab)
                    .UnderTransform(_intrudersParent));
        }

        private class BulletPool : MonoPoolableMemoryPool<BulletSpecs, Vector3, Quaternion, IMemoryPool, Bullet>
        {
        }

        private class IntruderPool : MonoPoolableMemoryPool<IntruderSpecs, Vector3, IMemoryPool, Intruder>
        {
        }
    }
}