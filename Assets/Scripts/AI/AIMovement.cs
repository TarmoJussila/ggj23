using System;
using Klonk.Platforming;
using UnityEngine;
using Random = System.Random;

namespace Klonk.AI
{
    [RequireComponent(typeof(FakeRigidbody))]  
    public class AIMovement : MovementBase
    {
        [Header("Settings")] 
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _minTurnTime;
        [SerializeField] private float _maxTurnTime;
        
        private FakeRigidbody _fakeRigidbody;
        private float _turnTime;
        private float _currentTurnTime;
        private Vector3 _currentDirection = Vector3.right;

        private void Awake()
        {
            _fakeRigidbody = GetComponent<FakeRigidbody>();
            _turnTime = UnityEngine.Random.Range(_minTurnTime, _maxTurnTime);
        }

        private void FixedUpdate()
        {
            Vector3 velocity = _fakeRigidbody.Velocity;
            velocity.x = _currentDirection.x * _moveSpeed;
            _currentTurnTime += Time.fixedDeltaTime;
            if (_turnTime < _currentTurnTime)
            {
                _currentDirection = -_currentDirection;
                _turnTime = UnityEngine.Random.Range(_minTurnTime, _maxTurnTime);
                _currentTurnTime = default;
            }

            _fakeRigidbody.SetVelocity(velocity);
            Vector3 scale = transform.localScale;
            scale.x = scale.x * _currentDirection.x > 0 ? scale.x : -scale.x;
            transform.localScale = scale;
        }
    }
}
