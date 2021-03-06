using UnityEngine;

namespace Kernel.Controls
{
    public class MoveInput
    {
        public float RollValue { get; set; }
        public float ForwardValue { get; set; }
        public float StrafeValue { get; set; }
        public float HoverValue { get; set; }
        public Vector2 LookPositionValue { get; set; }
        public bool Accelerated { get; set; }
    }
}