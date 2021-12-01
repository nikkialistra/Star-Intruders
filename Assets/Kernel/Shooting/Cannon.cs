using UnityEngine;

namespace Kernel.Shooting
{
    public abstract class Cannon : MonoBehaviour
    {
        public abstract Vector3 GetMuzzlePosition();
        public abstract void Shoot(Vector3 direction);
    }
}