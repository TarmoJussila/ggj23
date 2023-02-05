using System;
using Klonk.Platforming;
using UnityEngine;

namespace Klonk.AI
{
    [RequireComponent(typeof(FakeRigidbody))]  
    public class AIMovement : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnTime;
        
        private FakeRigidbody _fakeRigidbody;
        private float _currentTurnTime;
        private Vector3 _currentDirection = Vector3.right;

        private void Awake()
        {
            _fakeRigidbody = GetComponent<FakeRigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 velocity = _fakeRigidbody.Velocity;
            velocity.x = _currentDirection.x * _moveSpeed;
            _currentTurnTime += Time.fixedDeltaTime;
            if (_turnTime < _currentTurnTime)
            {
                _currentDirection = -_currentDirection;
                _currentTurnTime = default;
            }

            _fakeRigidbody.SetVelocity(velocity);
            Vector3 scale = transform.localScale;
            scale.x = scale.x * _currentDirection.x > 0 ? scale.x : -scale.x;
            transform.localScale = scale;
        }
    }
}
