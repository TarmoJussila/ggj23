using UnityEngine;

namespace Klonk.TileEntity
{
    public static class TileUtility
    {
        public const int TILES_PER_UNIT = 1;
        public const float TILE_SIZE = 1f / TILES_PER_UNIT;

        public static Vector2Int WorldToTileCoordinates(Vector3 worldCoordinates)
        {
            Vector3 coordinates = worldCoordinates * TILES_PER_UNIT;
            return new Vector2Int((int)coordinates.x, (int)coordinates.y);
        }

        public static Vector3 TileToWorldCoordinates(Vector2Int tileCoordinates)
        {
            Vector2Int transformed = tileCoordinates * (int)TILE_SIZE;
            return new Vector3(transformed.x, transformed.y, default);
        }

        public static void ExplosionInArea(Vector2Int center, int radius, ExplosionType type)
        {
            int centerX = center.x;
            int centerY = center.y;
            
            for (int x = centerX - radius; x < centerX + radius; x++)
            {
                for (int y = centerY - radius; y < centerY + radius; y++)
                {
                    if ((Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2)) >= Mathf.Pow(radius, 2))
                    {
                        continue;
                    }

                    if (TileEntityHandler.Instance.TryGetTileEntityAtPosition(x, y, out var tile))
                    {
                        switch (type)
                        {
                            case ExplosionType.Destroy:
                            {
                                tile.ReduceHealth(100);
                                break;
                            }
                            case ExplosionType.Freeze:
                            {
                                tile.SetSolid(SolidType.Rock);
                                break;
                            }
                            case ExplosionType.Liquify:
                            {
                                tile.SetLiquid(LiquidType.Water); 
                                break;
                            }
                            case ExplosionType.None:
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
