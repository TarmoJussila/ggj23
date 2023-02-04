using System;
using Klonk.Rendering;
using Klonk.TileEntity.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Klonk.TileEntity
{
    public class TileEntityHandler : MonoBehaviour
    {
        public static TileEntityHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TileEntityHandler>();
                }

                return _instance;
            }
        }
        private static TileEntityHandler _instance;

        public TileEntity[,] TileEntities => _tileEntities;
        public TileEntityGenerationData GenerationData => _generationData;
        public TileEntityData EntityData => _entityData;

        private TileEntity[,] _tileEntities;

        [SerializeField] private TileEntityGenerationData _generationData;
        [SerializeField] private TileEntityData _entityData;
        [SerializeField] private bool _drawGizmos = false;

        private int _updateIteration = 0;
        private int _worldWidth;
        private int _worldHeight;

        private void Awake()
        {
            _tileEntities = new TileEntity[_generationData.GenerationWidth, _generationData.GenerationHeight];
            _worldWidth = _tileEntities.GetLength(0);
            _worldHeight = _tileEntities.GetLength(1);
            GenerateTileEntities(_generationData);
        }

        private void GenerateTileEntities(TileEntityGenerationData generationData)
        {
            for (int i = 0; i < generationData.RockPatchGenerationAmount; i++)
            {
                Vector2Int position = new Vector2Int
                (
                    Random.Range(0, generationData.GenerationWidth),
                    Random.Range(0, generationData.GenerationHeight)
                );
                for (int j = 0; j < generationData.RockGenerationAmount; j++)
                {
                    do
                    {
                        position = new Vector2Int
                        (
                            Random.Range
                            (
                                Mathf.Clamp(position.x - 1, default, _generationData.GenerationWidth),
                                Mathf.Clamp(position.x + 2, default, _generationData.GenerationWidth)
                            ),
                            Random.Range
                            (
                                Mathf.Clamp(position.y - 1, default, _generationData.GenerationHeight),
                                Mathf.Clamp(position.y + 2, default, _generationData.GenerationHeight)
                            )
                        );
                    } while (TryGetTileEntityAtPosition(position, out _));

                    var tileEntity = new TileEntity(position, LiquidType.None, SolidType.Rock, ExplosionType.None);
                    _tileEntities[position.x, position.y] = tileEntity;
                }
            }

            for (int i = 0; i < generationData.SandPatchGenerationAmount; i++)
            {
                Vector2Int position = new Vector2Int
                (
                    Random.Range(0, generationData.GenerationWidth),
                    Random.Range(0, generationData.GenerationHeight)
                );
                for (int j = 0; j < generationData.SandGenerationAmount; j++)
                {
                    do
                    {
                        position = new Vector2Int
                        (
                            Random.Range
                            (
                                Mathf.Clamp(position.x - 1, default, _generationData.GenerationWidth),
                                Mathf.Clamp(position.x + 2, default, _generationData.GenerationWidth)
                            ),
                            Random.Range
                            (
                                Mathf.Clamp(position.y - 1, default, _generationData.GenerationHeight),
                                Mathf.Clamp(position.y + 2, default, _generationData.GenerationHeight)
                            )
                        );
                    } while (TryGetTileEntityAtPosition(position, out _));

                    var tileEntity = new TileEntity(position, LiquidType.None, SolidType.Sand, ExplosionType.None);
                    _tileEntities[position.x, position.y] = tileEntity;
                }
            }

            for (int i = 0; i < generationData.WaterSpawnSourceAmount; i++)
            {
                Vector2Int position;
                bool foundEmptyTile;
                bool foundTileAbove;
                do
                {
                    position = new Vector2Int(Random.Range(0, generationData.GenerationWidth), Random.Range(0, generationData.GenerationHeight));
                    foundEmptyTile = !TryGetTileEntityAtPosition(position, out _);
                    foundTileAbove = TryGetTileEntityAtPosition(position + new Vector2Int(0, 1), out _);
                } while (!(foundEmptyTile && foundTileAbove));

                var tileEntity = new TileEntity(position, LiquidType.Water, SolidType.None, ExplosionType.None, true);
                _tileEntities[position.x, position.y] = tileEntity;
            }

            for (int i = 0; i < generationData.AcidSpawnSourceAmount; i++)
            {
                Vector2Int position;
                bool foundEmptyTile;
                bool foundTileAbove;
                do
                {
                    position = new Vector2Int(Random.Range(0, generationData.GenerationWidth), Random.Range(0, generationData.GenerationHeight));
                    foundEmptyTile = !TryGetTileEntityAtPosition(position, out _);
                    foundTileAbove = TryGetTileEntityAtPosition(position + new Vector2Int(0, 1), out _);
                } while (!(foundEmptyTile && foundTileAbove));

                var tileEntity = new TileEntity(position, LiquidType.Acid, SolidType.None, ExplosionType.None, true);
                _tileEntities[position.x, position.y] = tileEntity;
            }
        }

        public bool TryAddTileToPosition(TileEntity tile, Vector2Int position)
        {
            if (TryGetTileEntityAtPosition(position, out _))
            {
                return false;
            }

            _tileEntities[position.x, position.y] = tile;
            return true;
        }

        public bool TryGetTileEntityAtPosition(Vector2Int position, out TileEntity tile) =>
            TryGetTileEntityAtPosition(position.x, position.y, out tile);

        public bool TryGetTileEntityAtPosition(int x, int y, out TileEntity tile)
        {
            if (x < 0 || x >= _worldWidth || y < 0 || y >= _worldHeight)
            {
                tile = null;
                return false;
            }

            tile = _tileEntities[x, y];
            return tile != null;
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < _tileEntities.GetLength(0) && y >= 0 && y < _tileEntities.GetLength(1);
        }

        private void FixedUpdate()
        {
            _updateIteration++;
            
            if (_tileEntities != null)
            {
                Vector3 camPos = WorldRenderer.Instance.Camera.transform.position;
                Vector3Int camPosInt = new(Mathf.RoundToInt(camPos.x), Mathf.RoundToInt(camPos.y));

                int width = WorldRenderer.Instance.Width + 4; //maek thing go out screen
                int height = WorldRenderer.Instance.Height + 4;
                
                for (int x = camPosInt.x; x < camPosInt.x + width; x++)
                {
                    for (int y = camPosInt.y; y < camPosInt.y + height; y++)
                    {
                        if (!TryGetTileEntityAtPosition(x, y, out TileEntity tileEntity)
                            || tileEntity.LastUpdateFrame == _updateIteration)
                        {
                            continue;
                        }
                        else
                        {
                            if (tileEntity.Health <= 0 || tileEntity.Potency <= 0)
                            {
                                _tileEntities[x, y] = null;
                                continue;
                            }
                            if (tileEntity.IsSolid)
                            {
                                continue;
                            }
                        }

                        var position = tileEntity.UpdateEntity(_updateIteration);

                        if (tileEntity.TileData.IsSpawnSource)
                        {
                            if (!TryGetTileEntityAtPosition(position.x, position.y, out TileEntity _))
                            {
                                var newTileEntity = new TileEntity(position, tileEntity.TileData.SpawnLiquidType, SolidType.None, ExplosionType.None);
                                _tileEntities[position.x, position.y] = newTileEntity;
                            }
                        }
                        else
                        {
                            if (position.x != x || position.y != y)
                            {
                                _tileEntities[x, y] = null;
                                _tileEntities[position.x, position.y] = tileEntity;
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 point = (UnityEngine.Input.mousePosition / WorldRenderer.Instance.TextureResDivider + WorldRenderer.Instance.Camera.transform.position);
                TileUtility.ExplosionInArea(new Vector2Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y)), 10, ExplosionType.Destroy);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 point = (UnityEngine.Input.mousePosition / WorldRenderer.Instance.TextureResDivider + WorldRenderer.Instance.Camera.transform.position);
                TileUtility.ExplosionInArea(new Vector2Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y)), 10, ExplosionType.Destroy);
            }
        }

        private void OnDrawGizmos()
        {
            if (_tileEntities != null && _drawGizmos)
            {
                for (int x = 0; x < _worldWidth; x++)
                {
                    for (int y = 0; y < _worldHeight; y++)
                    {
                        if (TryGetTileEntityAtPosition(x, y, out TileEntity tileEntity))
                        {
                            Gizmos.color = tileEntity.TileColor;
                            Gizmos.DrawCube(new Vector2(x, y), Vector2.one);
                        }
                    }
                }
            }
        }
    }
}