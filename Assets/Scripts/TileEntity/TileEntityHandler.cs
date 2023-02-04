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
            GenerateTileEntities(_generationData);
            _worldWidth = _tileEntities.GetLength(0);
            _worldHeight = _tileEntities.GetLength(1);
        }

        private void GenerateTileEntities(TileEntityGenerationData generationData)
        {
            for (int i = 0; i < generationData.SandPatchGenerationAmount; i++)
            {
                Vector2Int position = new Vector2Int(
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

                    var tileEntity = new TileEntity(position, LiquidType.None, SolidType.Sand);
                    _tileEntities[position.x, position.y] = tileEntity;
                }
            }

            for (int i = 0; i < generationData.RockPatchGenerationAmount; i++)
            {
                Vector2Int position = new Vector2Int(
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

                    var tileEntity = new TileEntity(position, LiquidType.None, SolidType.Rock);
                    _tileEntities[position.x, position.y] = tileEntity;
                }
            }

            for (int i = 0; i < generationData.WaterGenerationAmount; i++)
            {
                Vector2Int position;
                do
                {
                    position = new Vector2Int(Random.Range(0, generationData.GenerationWidth), Random.Range(0, generationData.GenerationHeight));
                } while (TryGetTileEntityAtPosition(position, out _));

                var tileEntity = new TileEntity(position, LiquidType.Water, SolidType.None);
                _tileEntities[position.x, position.y] = tileEntity;
            }

            for (int i = 0; i < generationData.AcidGenerationAmount; i++)
            {
                Vector2Int position;
                do
                {
                    position = new Vector2Int(Random.Range(0, generationData.GenerationWidth), Random.Range(0, generationData.GenerationHeight));
                } while (TryGetTileEntityAtPosition(position, out _));

                var tileEntity = new TileEntity(position, LiquidType.Acid, SolidType.None);
                _tileEntities[position.x, position.y] = tileEntity;
            }
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

        private void FixedUpdate()
        {
            _updateIteration++;
            
            if (_tileEntities != null)
            {
                Vector3 camPos = WorldRenderer.Instance.Camera.transform.position;
                Vector3Int camPosInt = new(Mathf.RoundToInt(camPos.x), Mathf.RoundToInt(camPos.y));

                int width = WorldRenderer.Instance.Width;
                int height = WorldRenderer.Instance.Height;
                
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
                            if (tileEntity.Health <= 0)
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

                        if (position.x != x || position.y != y)
                        {
                            _tileEntities[x, y] = null;
                            _tileEntities[position.x, position.y] = tileEntity;
                        }
                    }
                }
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
                            var tileData = _entityData.GetTileDataForType(tileEntity.SolidType, tileEntity.LiquidType);
                            Gizmos.color = tileData.Color;
                            Gizmos.DrawCube(new Vector2(x, y), Vector2.one);
                        }
                    }
                }
            }
        }
    }
}