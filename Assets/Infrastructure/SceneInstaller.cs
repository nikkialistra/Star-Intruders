using Game.Cameras.Scripts;
using Game.Shooting.Cannons.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        [Title("Cameras"), Required]
        [SerializeField] private Camera _mainCamera;
        [Required]
        [SerializeField] private ShakeCameraOffset _shakeCameraOffset;
        [Required]
        [SerializeField] private CameraMovement _cameraMovement;

        [Title("Player"), Required]
        [SerializeField] private Transform _chasePoint;
        [Required]
        [SerializeField] private Transform _cockpitPoint;

        [Title("Shooting"), Required]
        [SerializeField] private Cannon _cannon;
        

        public override void InstallBindings()
        {
            BindCameras();
            BindPlayer();
            BindShooting();
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

        private void BindShooting()
        {
            Container.BindInstance(_cannon);
        }
    }
}