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
        public Color[] ColorPalette;
        public float Gravity;
        public int Health;

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
            return tileDatas.Find(x => x.SolidType == type && x.SolidType != SolidType.None);
        }

        public TileData GetTileDataForType(LiquidType type)
        {
            return tileDatas.Find(x => x.LiquidType == type && x.LiquidType != LiquidType.None);
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