using System;
using UnityEngine;

namespace Klonk.TileEntity
{
    public static class TileUtility
    {
        public const int TILES_PER_UNIT = 1;
        public const float TILE_SIZE = 1f / TILES_PER_UNIT;

        private static Collider2D[] _overlapArray = new Collider2D[8];

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

        public static void ExplosionInArea(Vector3 center, int radius, ExplosionType type) =>
            ExplosionInArea(new Vector2Int(Mathf.FloorToInt(center.x), Mathf.FloorToInt(center.y)), radius, type);

        public static void ExplosionInArea(Vector2Int center, int radius, ExplosionType type)
        {
            int centerX = center.x;
            int centerY = center.y;

            Physics2D.OverlapCircleNonAlloc(center, radius, _overlapArray);
            
            ParticleHandler.Instance.PlayExplosion(new Vector3(center.x, center.y, 0), type);

            foreach (Collider2D col in _overlapArray)
            {
                if (col != null && col.TryGetComponent<CharacterHealth>(out var component))
                {
                    switch (type)
                    {
                        case ExplosionType.Destroy:
                        {
                            component.TakeDamage(500);
                            break;
                        }
                        case ExplosionType.Freeze:
                        {
                            component.Freeze();
                            break;
                        }
                        case ExplosionType.Liquify:
                        {
                            component.TakeDamage(500);
                            break;
                        }
                        case ExplosionType.None: break;
                    }
                }
            }

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
                                TileEntityHandler.Instance.RemoveAt(x,y);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
