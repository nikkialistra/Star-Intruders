using UnityEngine;

namespace Game.Shooting.Bullets
{
    public class BulletSpecs
    {
        public float Lifetime { get; set; }
        public int Damage { get; set; }

        public float MoveSpeed { get; set; }
        public Vector3 Direction { get; set; }
    }
}