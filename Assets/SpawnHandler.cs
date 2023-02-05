using System;
using System.Collections;
using System.Collections.Generic;
using Klonk.TileEntity;
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
        [SerializeField] private CharacterHealth[] enemies = new CharacterHealth[200];
        [SerializeField] private int spawnMargin = 50;

        private void Awake()
        {
            Instance = this;
        }

        public void SpawnCharacters()
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                GameObject go = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);
                int x = Random.Range(spawnMargin, TileEntityHandler.Instance.GenerationData.GenerationWidth - spawnMargin);
                int y = Random.Range(spawnMargin, TileEntityHandler.Instance.GenerationData.GenerationHeight - spawnMargin);

                Vector2Int spawnPos = new Vector2Int(x, y);
                TileUtility.ExplosionInArea(spawnPos, 15, ExplosionType.None);

                go.transform.position.Set(spawnPos.x, spawnPos.y, 0);
            }
        }
    }
}
