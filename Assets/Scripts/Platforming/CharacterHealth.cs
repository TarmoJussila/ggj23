using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Klonk.AI;
using Klonk.Platforming;
using Klonk.TileEntity;
using Unity.Mathematics;
using UnityEngine;

namespace Klonk
{
    public class CharacterHealth : MonoBehaviour
    {
        public static event System.Action OnEnemyDead;
        
        [SerializeField] private int _health = 1000;
        
        private OverlapChecker _overlapChecker;
        private MovementBase _movement;
        private SpriteRenderer _renderer;

        private Coroutine _freezeCoroutine;
        
        private void Start()
        {
            _overlapChecker = GetComponent<OverlapChecker>();
            _movement = GetComponent<MovementBase>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (_health > 0)
            {
                CheckDamageOverlapDamage();
            }
        }

        private void CheckDamageOverlapDamage()
        {
            _health -= _overlapChecker.OverlapLiquids.FindAll(l => l == LiquidType.Acid).Count;
            _health -= _overlapChecker.OverlapSolids.Count > 5 ? 1 : 0;
            
            CheckHealth();
        }

        public bool TakeDamage(int amount)
        {
            if (_health <= 0)
            {
                return false;
            }
            _health -= amount;
            return CheckHealth();
        }

        public void Freeze()
        {
            if (_freezeCoroutine != null)
            {
                StopCoroutine(_freezeCoroutine);
            }
            _freezeCoroutine = StartCoroutine(FreezeEnumerator());
        }

        private bool CheckHealth()
        {
            bool alive = _health > 0;

            if (!alive)
            {
                if (GetComponentInChildren<AIMovement>(true) != null)
                {
                    OnEnemyDead?.Invoke();
                }
                StartCoroutine(DeathEffect());
                _movement.enabled = false;
            }
            
            return alive;
        }

        private IEnumerator DeathEffect()
        {
            float t = 0.0f;
            while (t < 1.0f)
            {
                t = Mathf.Clamp01(t + Time.deltaTime * 3);
                _renderer.color = Color.Lerp(Color.white, Color.red, t);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            _renderer.enabled = false;
        }

        private IEnumerator FreezeEnumerator()
        {
            _movement.enabled = false;
            _renderer.color = Color.blue;
            yield return new WaitForSeconds(5.0f);
            _movement.enabled = true;
            _renderer.color = Color.white;
        }
    }
}
