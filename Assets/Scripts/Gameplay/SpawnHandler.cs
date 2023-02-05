using System;
using System.Collections;
using System.Collections.Generic;
using Klonk.Platforming;
using Klonk.TileEntity;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Klonk
{
    public class SpawnHandler : MonoBehaviour
    {
        public static SpawnHandler Instance;
        
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private int _enemyCount = 200;
        [SerializeField] private int spawnMargin = 50;
        
        private CharacterHealth[] enemies = new CharacterHealth[200];

        private void Awake()
        {
            Instance = this;
        }

        public void SpawnCharacters()
        {
            Vector3 playerpos = PlayerMovement.Instance.transform.position;
            TileUtility.ExplosionInArea(playerpos, 25, ExplosionType.None);
            
            for (int i = 0; i < _enemyCount; i++)
            {
                int x = Random.Range(spawnMargin, TileEntityHandler.Instance.GenerationData.GenerationWidth - spawnMargin);
                int y = Random.Range(spawnMargin, TileEntityHandler.Instance.GenerationData.GenerationHeight - spawnMargin);

                Vector2Int spawnPos = new Vector2Int(x, y);
                TileUtility.ExplosionInArea(spawnPos, 25, ExplosionType.None);

                Vector3 pos = new Vector3(spawnPos.x, spawnPos.y, 0);
                
                GameObject go = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)], pos, quaternion.identity);
            }
        }
    }
}
