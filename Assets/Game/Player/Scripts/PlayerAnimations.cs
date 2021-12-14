using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [Required]
        [SerializeField] private Animator _animator;

        [ValidateInput("@$value.Count > 0")]
        [SerializeField] private List<SkinnedMeshRenderer> _engines;
        [Required]
        [SerializeField] private Material _enabledEngine;
        [Required]
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