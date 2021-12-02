using Game.Cameras.Scripts;
using Game.Shooting.Cannons.Scripts;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        [Header("Cameras")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private ShakeCameraOffset _shakeCameraOffset;
        

        [Header("Shooting")]
        [SerializeField] private Cannon _cannon;
        

        public override void InstallBindings()
        {
            BindCameras();
            BindShooting();
        }

        private void BindCameras()
        {
            Container.BindInstance(_mainCamera);
            Container.BindInstance(_shakeCameraOffset);
        }

        private void BindShooting()
        {
            Container.BindInstance(_cannon);
        }
    }
}