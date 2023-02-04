using Klonk.TileEntity.Data;
using UnityEngine;

namespace Klonk.TileEntity
{
    public enum SolidType { None, Rock, Sand }
    public enum LiquidType { None, Water, Acid }
    
    public class TileEntity
    {
        public Vector2Int Position { get; private set; }
        public SolidType SolidType { get; private set; }
        public LiquidType LiquidType { get; private set; }
        public bool IsSolid { get { return SolidType != SolidType.None; } }
        public bool IsLiquid { get { return LiquidType != LiquidType.None; } }
        public float Gravity { get; private set; }
        public Vector2 Velocity { get; private set; }

        public TileData TileData { get; private set; }

        public int LastUpdateFrame { get; private set; } = -1;
        
        public TileEntity(Vector2Int position, LiquidType liquidType, SolidType solidType, float gravity = 0f)
        {
            TileData = TileEntityHandler.Instance.EntityData.GetTileDataForType(solidType, liquidType);
            Position = position;
            SolidType = solidType;
            LiquidType = liquidType;
            Gravity = gravity;
        }

        public Vector2Int UpdateEntity(int updateFrame)
        {
            if (IsLiquid)
            {
                if (!TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Mathf.Clamp(Position.x, default, TileEntityHandler.Instance.GenerationData.GenerationWidth), Mathf.Max(Position.y - 1, default)), out _))
                {
                    Position = new Vector2Int(Mathf.Clamp(Position.x, default, TileEntityHandler.Instance.GenerationData.GenerationWidth), Mathf.Max(Position.y - 1, default));
                    return Position;
                }
                int direction = Random.Range(0, 2) == 0 ? -1 : 1;
                if (!TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Mathf.Clamp(Position.x + direction, default, TileEntityHandler.Instance.GenerationData.GenerationWidth), Mathf.Max(Position.y, default)), out _))
                {
                    Position = new Vector2Int(Mathf.Clamp(Position.x + direction, default, TileEntityHandler.Instance.GenerationData.GenerationWidth), Mathf.Max(Position.y, default));
                    return Position;
                }
            }
            if (IsSolid)
            {

            }
            LastUpdateFrame = updateFrame;
            return Position;
        }
    }
}