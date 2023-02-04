using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klonk.TileEntity.Data
{
    [Serializable]
    public class TileData
    {
        public SolidType SolidType;
        public LiquidType LiquidType;
        public Color Color;
        public float Gravity;

        public int TextureIndex;
        public Vector2 UvMin;
        public Vector2 UvMax;
    }

    [CreateAssetMenu(fileName = nameof(TileEntityData), menuName = nameof(TileEntityData))]
    public class TileEntityData : ScriptableObject
    {
        [SerializeField] private List<TileData> tileDatas = new List<TileData>();

        public TileData GetTileDataForType(SolidType type)
        {
            return tileDatas.Find(x => x.SolidType == type);
        }

        public TileData GetTileDataForType(LiquidType type)
        {
            return tileDatas.Find(x => x.LiquidType == type);
        }

        public TileData GetTileDataForType(SolidType solidType, LiquidType liquidType)
        {
            var tileData = GetTileDataForType(solidType);
            if (tileData == null)
            {
                tileData = GetTileDataForType(liquidType);
            }
            return tileData;
        }
    }
}