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

        public EntityDef EntityDefinition { get; private set; }

        public int lastUpdate = -1;
        
        public TileEntity(Vector2Int position, LiquidType liquidType, SolidType solidType, float gravity = 0f)
        {
            var defs = Resources.LoadAll<EntityDef>("");
            EntityDefinition = defs[Random.Range(0, defs.Length)];
            
            Position = position;
            SolidType = solidType;
            LiquidType = liquidType;
            Gravity = gravity;
        }

        public Vector2Int UpdateEntity()
        {
            TileEntity tileEntity;
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
            return Position;
        }
    }
}