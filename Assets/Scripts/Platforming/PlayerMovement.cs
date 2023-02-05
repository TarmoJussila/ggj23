using UnityEngine;
using UnityEngine.InputSystem;

namespace Klonk.Platforming
{
    public class MovementBase : MonoBehaviour
    {
        
    }
    
    [RequireComponent(typeof(FakeRigidbody))]
    public class PlayerMovement : MovementBase
    {
        public static PlayerMovement Instance;
        private const int AlternativeSpriteCount = 2;
        private const float WalkSpriteInterval = 0.1f;
        
        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 1f; 
        [SerializeField] private int _jumpTickCount;
        [SerializeField] private float _jumpStartVelocity; 
        [SerializeField] private float _jumpEndVelocity; 
        [SerializeField] private float _jumpVelocity;

        [Header("Sprites")]
        [SerializeField] private Sprite[] _lookForward;
        [SerializeField] private Sprite[] _lookUp;
        [SerializeField] private Sprite[] _lookDown;

        private int _jumpTicks;
        private bool _isJumping;
        private float _horizontalInput;
        private float _verticalInput;
        private int _spriteIndex;
        private float _walkTimer;

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
        
        private FakeRigidbody _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private void Update()
        {
            if (Mathf.Approximately(0, _horizontalInput))
            {
                return;
            }

            _walkTimer += Time.deltaTime;
            if (_walkTimer < WalkSpriteInterval)
            {
                return;
            }

            _walkTimer = 0;
            
            if (CheckAlternativeSprite(_lookForward))
            {
                return;
            }

            if (CheckAlternativeSprite(_lookDown))
            {
                return;
            }

            CheckAlternativeSprite(_lookUp);
        }
        
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
            Sprite nextSprite = _lookForward[0];
            
            if (_verticalInput < 0)
            {
                nextSprite = _lookDown[0];
            }

            if (_verticalInput > 0)
            {
                nextSprite = _lookUp[0];
            }

            if (_spriteRenderer.sprite != nextSprite)
            {
                _spriteRenderer.sprite = nextSprite;
            }
        }

        public void OnHorizontalInput(InputAction.CallbackContext context)
        {
            float previous = _horizontalInput;
            _horizontalInput = context.ReadValue<float>();

            if (Mathf.Approximately(_horizontalInput, default))
            {
                return;
            }
            
            Vector3 scale = transform.localScale;
            scale.x = _horizontalInput * scale.x > 0 ? scale.x : -scale.x;
            transform.localScale = scale;
        }

        private bool CheckAlternativeSprite(Sprite[] sprites)
        {
            Sprite current = _spriteRenderer.sprite;
            foreach (Sprite sprite in sprites)
            {
                if (current.name == sprite.name)
                {
                    _spriteIndex++;
                    _spriteIndex = _spriteIndex >= AlternativeSpriteCount ? 0 : _spriteIndex;
                    _spriteRenderer.sprite = sprites[_spriteIndex];
                    
                    return true;
                }
            }

            return false;
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
