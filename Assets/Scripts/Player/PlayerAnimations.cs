using UnityEngine;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
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
        }

        public void Land()
        {
            _animator.SetBool(_takenOff, false);
        }
    }
}