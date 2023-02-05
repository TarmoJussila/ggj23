using System;
using System.Collections;
using System.Collections.Generic;
using Klonk.TileEntity;
using UnityEngine;

namespace Klonk
{
    public class ParticleHandler : MonoBehaviour
    {
        public static ParticleHandler Instance;

        [SerializeField] private ParticleSystem _explosionParticle;
        [SerializeField] private ParticleSystem _freezeParticle;
        [SerializeField] private ParticleSystem _liquifyParticle;
        [SerializeField] private ParticleSystem _bloodParticle;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayExplosion(Vector3 position, ExplosionType type)
        {
            GameObject prefab = _explosionParticle.gameObject;
            switch (type)
            {
                case ExplosionType.Destroy:
                {
                    prefab = _explosionParticle.gameObject;
                    break;
                }
                case ExplosionType.Freeze:
                {
                    prefab = _freezeParticle.gameObject;
                    break;
                }
                case ExplosionType.Liquify:
                {
                    prefab = _liquifyParticle.gameObject;
                    break;
                }
                default: break;
            }

            GameObject go = Instantiate(prefab, position, Quaternion.identity);
        }

        public void PlayBlood(Vector3 position)
        {
            GameObject go = Instantiate(_bloodParticle.gameObject, position, Quaternion.identity);
        }
    }
}
