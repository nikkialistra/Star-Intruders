using Kernel.Shooting;
using UnityEngine;
using Zenject;

namespace Kernel.Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        [Header("Scene")]
        [SerializeField] private Camera _mainCamera;

        [Header("Shooting")]
        [SerializeField] private Cannon _cannon;
        

        public override void InstallBindings()
        {
            BindScene();

            BindShooting();
        }

        private void BindScene()
        {
            Container.BindInstance(_mainCamera);
        }

        private void BindShooting()
        {
            Container.BindInstance(_cannon);
        }
    }
}