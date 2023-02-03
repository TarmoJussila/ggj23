using UnityEngine;

namespace Klonk.TileEntity
{
    public class TileEntity
    {
        public Vector2Int Position { get; private set; }
        public bool IsLiquid { get; private set; }
        public bool IsSolid { get; private set; }
        public float Gravity { get; private set; }

        public EntityDef EntityDefinition { get; private set; }
        
        public TileEntity(Vector2Int position, bool isLiquid, bool isSolid, float gravity)
        {
            var defs = Resources.LoadAll<EntityDef>("");
            EntityDefinition = defs[Random.Range(0, defs.Length)];
            
            Position = position;
            IsLiquid = isLiquid;
            IsSolid = isSolid;
            Gravity = gravity;
        }

        public Vector2Int UpdateEntity()
        {
            TileEntity tileEntity;
            if (IsLiquid)
            {
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Mathf.Clamp(Position.x, default, TileEntityHandler.Instance.GenerationData.GenerationWidth), Mathf.Max(Position.y - 1, default)));
                if (tileEntity == null)
                {
                    Position = new Vector2Int(Mathf.Clamp(Position.x, default, TileEntityHandler.Instance.GenerationData.GenerationWidth), Mathf.Max(Position.y - 1, default));
                    return Position;
                }
                int direction = Random.Range(0, 2) == 0 ? -1 : 1;
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Mathf.Clamp(Position.x + direction, default, TileEntityHandler.Instance.GenerationData.GenerationWidth), Mathf.Max(Position.y, default)));
                if (tileEntity == null)
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