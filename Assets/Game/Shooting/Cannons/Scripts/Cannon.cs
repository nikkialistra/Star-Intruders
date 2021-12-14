using UnityEngine;

namespace Game.Shooting.Cannons
{
    public abstract class Cannon : MonoBehaviour
    {
        public abstract Vector3 GetMuzzlePosition();
        public abstract void Shoot(Vector3 direction);
    }
}