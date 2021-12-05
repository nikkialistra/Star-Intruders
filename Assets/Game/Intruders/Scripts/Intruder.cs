using UnityEngine;

namespace Game.Intruders.Scripts
{
    public class Intruder : MonoBehaviour, IIntruderData
    {
        private IntruderSpecs _specs;

        public void Initialize(IntruderSpecs specs)
        {
            _specs = specs;
        }
        
        public IntruderSpecs GetSpecs()
        {
            return _specs;
        }
    }
}