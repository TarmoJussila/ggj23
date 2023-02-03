using UnityEngine;

namespace Klonk.TileEntity
{
    public class TileEntity
    {
        public Vector2Int Position { get; private set; }
        public bool IsLiquid { get; private set; }
        public bool IsSolid { get; private set; }
        public float Gravity { get; private set; }
        
        public TileEntity()
        {
        }

        public TileEntity(Vector2Int position, bool isLiquid, bool isSolid, float gravity)
        {
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
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Mathf.Max(Position.x, default), Mathf.Max(Position.y - 1, default)));
                if (tileEntity == null)
                {
                    Position = new Vector2Int(Mathf.Max(Position.x, default), Mathf.Max(Position.y - 1, default));
                    return Position;
                }
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Mathf.Max(Position.x - 1, default), Mathf.Max(Position.y, default)));
                if (tileEntity == null)
                {
                    Position = new Vector2Int(Mathf.Max(Position.x - 1, default), Mathf.Max(Position.y, default));
                    return Position;
                }
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Mathf.Max(Position.x + 1, default), Mathf.Max(Position.y, default)));
                if (tileEntity == null)
                {
                    Position = new Vector2Int(Mathf.Max(Position.x + 1, default), Mathf.Max(Position.y, default));
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