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

        public void UpdateEntity()
        {
            TileEntity tileEntity;
            if (IsLiquid)
            {
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Position.x, Position.y - 1));
                if (tileEntity == null)
                {
                    Position = new Vector2Int(Position.x, Position.y - 1);
                    return;
                }
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Position.x - 1, Position.y));
                if (tileEntity == null)
                {
                    Position = new Vector2Int(Position.x - 1, Position.y);
                    return;
                }
                tileEntity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(new Vector2Int(Position.x + 1, Position.y));
                if (tileEntity == null)
                {
                    Position = new Vector2Int(Position.x + 1, Position.y);
                    return;
                }
            }
            if (IsSolid)
            {

            }
        }
    }
}