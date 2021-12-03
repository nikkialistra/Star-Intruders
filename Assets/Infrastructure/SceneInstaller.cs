using Game.Cameras.Scripts;
using Game.Shooting.Cannons.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        [Title("Cameras")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private ShakeCameraOffset _shakeCameraOffset;
        
        [Title("Player")]
        [SerializeField] private Transform _chasePoint;
        [SerializeField] private Transform _cockpitPoint;

        [Title("Shooting")]
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