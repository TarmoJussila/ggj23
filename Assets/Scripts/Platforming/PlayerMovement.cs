using UnityEngine;
using UnityEngine.InputSystem;

namespace Klonk.Platforming
{
    [RequireComponent(typeof(FakeRigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 1f; 
        [SerializeField] private int _jumpTickCount;
        [SerializeField] private float _jumpVelocity; 

        private int _jumpTicks;
        private bool _isJumping;
        private float _horizontalMovement;
        private FakeRigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<FakeRigidbody>();
        }

        private void FixedUpdate()
        {
            _isJumping = _jumpTicks-- > 0;
            Vector2 velocity = _rigidbody.Velocity;
            velocity.y = _isJumping ? _jumpVelocity : velocity.y;
            velocity.x = _horizontalMovement * _moveSpeed / 10f;
            _rigidbody.SetVelocity(velocity);
        }

        public void OnHorizontalInput(InputAction.CallbackContext context)
        {
            _horizontalMovement = context.ReadValue<float>();
            Vector3 scale = transform.localScale;
            scale.x = scale.x * _horizontalMovement < 0 ? -scale.x : scale.x;
            transform.localScale = scale;
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            Debug.Log("jump start");
            Debug.Log("started " + context.started);
            Debug.Log("performed " + context.performed);
            Debug.Log("canceled " + context.canceled);
            
            if (context.started && _rigidbody.IsGrounded)
            {
                _isJumping = true;
                _jumpTicks = _jumpTickCount;
            }
            else if (context.canceled && _isJumping)
            {
                _isJumping = false;
                _jumpTicks = default;
            }
        }
    }
}
