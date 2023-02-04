using UnityEngine;
using UnityEngine.InputSystem;

namespace Klonk.Platforming
{
    [RequireComponent(typeof(FakeRigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance;
        
        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 1f; 
        [SerializeField] private int _jumpTickCount;
        [SerializeField] private float _jumpStartVelocity; 
        [SerializeField] private float _jumpEndVelocity; 

        private int _jumpTicks;
        private bool _isJumping;
        private float _horizontalInput;
        private float _verticalInput;

        public FakeRigidbody Rigidbody { get; private set; }

        public float HorizontalInput { get { return _horizontalInput; } }
        public float VerticalInput { get { return _verticalInput; } }

        private void Awake()
        {
            Instance = this;
            Rigidbody = GetComponent<FakeRigidbody>();
            _rigidbody = GetComponent<FakeRigidbody>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private float _horizontalMovement;
        private FakeRigidbody _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private void FixedUpdate()
        {
            _isJumping = _jumpTicks-- > 0;
            Vector2 velocity = Rigidbody.Velocity;
            float jumpVelocity = Mathf.Lerp(_jumpStartVelocity, _jumpEndVelocity, _jumpTicks / (float)_jumpTickCount);
            velocity.y = _isJumping ? jumpVelocity : velocity.y;
            velocity.x = _horizontalInput * _moveSpeed / 10f;
            Rigidbody.SetVelocity(velocity);
        }

        public void OnVerticalInput(InputAction.CallbackContext context)
        {
            _verticalInput = context.ReadValue<float>();
        }

        public void OnHorizontalInput(InputAction.CallbackContext context)
        {
            _horizontalInput = context.ReadValue<float>();
            float previous = _horizontalMovement;
            _horizontalMovement = context.ReadValue<float>();

            if (Mathf.Approximately(0, _horizontalMovement))
            {
                return;
            }

            if (!Mathf.Approximately(previous, _horizontalMovement))
            {
                _spriteRenderer.flipX = _horizontalMovement < 0;
            }
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            Debug.Log("jump start");
            Debug.Log("started " + context.started);
            Debug.Log("performed " + context.performed);
            Debug.Log("canceled " + context.canceled);
            
            if (context.started && Rigidbody.IsGrounded)
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

        public int GetHorizontalDirection()
        {
            return _spriteRenderer.flipX ? -1 : 1;
        }
    }
}
