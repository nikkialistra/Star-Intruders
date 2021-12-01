using System.Collections.Generic;
using UnityEngine;

namespace Kernel.Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private List<SkinnedMeshRenderer> _engines;
        [SerializeField] private Material _enabledEngine;
        [SerializeField] private Material _disabledEngine;

        private readonly int _stopped = Animator.StringToHash("stopped");
        private readonly int _takenOff = Animator.StringToHash("takenOff");

        public void Move()
        {
            _animator.SetBool(_stopped, false);
        }

        public void Stop()
        {
            _animator.SetBool(_stopped, true);
        }

        public void TakeOff()
        {
            _animator.SetBool(_takenOff, true);
            TurnOnEngines();
        }

        private void TurnOnEngines()
        {
            foreach (var engine in _engines)
            {
                var materials = engine.materials;
                materials[1] = _enabledEngine;
                engine.materials = materials;
            }
        }

        public void Land()
        {
            _animator.SetBool(_takenOff, false);
        }

        public void TurnOffEngines()
        {
            foreach (var engine in _engines)
            {
                var materials = engine.materials;
                materials[1] = _disabledEngine;
                engine.materials = materials;
            }
        }
    }
}