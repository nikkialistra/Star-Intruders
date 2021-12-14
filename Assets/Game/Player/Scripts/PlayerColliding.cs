using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerColliding : MonoBehaviour
    {
        [SerializeField] private float _colllisionDrag;
        [SerializeField] private float _collisionAngularDrag;

        private float _defaultDrag;
        private float _defaultAngularDrag;

        private Rigidbody _rigidbody;
        private PlayerMovement _movement;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            _defaultDrag = _rigidbody.drag;
            _defaultAngularDrag = _rigidbody.angularDrag;
        }

        private void OnCollisionEnter(Collision other)
        {
            _rigidbody.drag = _colllisionDrag;
            _rigidbody.angularDrag = _collisionAngularDrag;
            _movement.TakeAwayControl();
        }

        private void OnCollisionExit(Collision other)
        {
            _rigidbody.drag = _defaultDrag;
            _rigidbody.angularDrag = _defaultAngularDrag;
            _movement.ReturnControl();
        }
    }
}