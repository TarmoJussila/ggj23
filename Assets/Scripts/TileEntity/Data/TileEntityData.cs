using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klonk.TileEntity.Data
{
    [Serializable]
    public class TileData
    {
        [Header("Tile Data")]
        public SolidType SolidType;
        public LiquidType LiquidType;
        public ExplosionType ExplosionType;
        public Color[] ColorPalette;
        public float Gravity;
        public int Health = 1;
        public bool IsAcidResistant;
        public int Potency = 1;

        [Header("Spawn Source Data")]
        public bool IsSpawnSource;
        public LiquidType SpawnLiquidType;
        public int SpawnSpeed;
        public int SpawnLifespan;
        public bool IsEndlessLifespan;
    }

    [CreateAssetMenu(fileName = nameof(TileEntityData), menuName = nameof(TileEntityData))]
    public class TileEntityData : ScriptableObject
    {
        [SerializeField] private List<TileData> tileDatas = new List<TileData>();

        public TileData GetTileDataForType(SolidType type)
        {
            return tileDatas.Find(x => x.SolidType == type && x.SolidType != SolidType.None);
        }

        public TileData GetTileDataForType(LiquidType type, bool isSpawnSource = false)
        {
            return tileDatas.Find(x => x.LiquidType == type && x.LiquidType != LiquidType.None && x.IsSpawnSource == isSpawnSource);
        }

        public TileData GetTileDataForType(ExplosionType type)
        {
            return tileDatas.Find(x => x.ExplosionType == type && x.ExplosionType != ExplosionType.None);
        }

        public TileData GetTileDataForType(SolidType solidType, LiquidType liquidType, ExplosionType explosionType, bool isSpawnSource = false)
        {
            var tileData = GetTileDataForType(solidType);
            if (tileData == null)
            {
                tileData = GetTileDataForType(liquidType, isSpawnSource);
            }
            if (tileData == null)
            {
                tileData = GetTileDataForType(explosionType);
            }
            return tileData;
        }

        public TileData GetTileDataByIndex(int index)
        {
            return tileDatas[index];
        }
    }
}