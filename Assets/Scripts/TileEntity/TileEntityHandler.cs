using System.Collections.Generic;
using System.Linq;
using Klonk.TileEntity.Data;
using UnityEngine;

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

        public Dictionary<Vector2Int, TileEntity> TileEntities => _tileEntities;
        public TileEntityGenerationData GenerationData => _generationData;
        
        private Dictionary<Vector2Int, TileEntity> _tileEntities;

        [SerializeField] private TileEntityGenerationData _generationData;
        [SerializeField] private bool _drawGizmos = false;

        private void Awake()
        {
            _tileEntities = new Dictionary<Vector2Int, TileEntity>();
            GenerateTileEntities(_generationData);
        }

        private void GenerateTileEntities(TileEntityGenerationData generationData)
        {
            for (int i = 0; i < generationData.SolidPatchGenerationAmount; i++)
            {
                Vector2Int position = new Vector2Int(Random.Range(0, generationData.GenerationWidth), Random.Range(0, generationData.GenerationHeight));
                for (int j = 0; j < generationData.SolidGenerationAmount; j++)
                {
                    do
                    {
                        position = new Vector2Int(Random.Range(Mathf.Clamp(position.x - 1, default, _generationData.GenerationWidth), Mathf.Clamp(position.x + 2, default, _generationData.GenerationWidth)), Random.Range(Mathf.Clamp(position.y - 1, default, _generationData.GenerationHeight), Mathf.Clamp(position.y + 2, default, _generationData.GenerationHeight)));
                    } while (TryGetTileEntityAtPosition(position) != null);

                    var tileEntity = new TileEntity(position, false, true, 0f);
                    _tileEntities.Add(position, tileEntity);
                }
            }
            for (int i = 0; i < generationData.LiquidGenerationAmount; i++)
            {
                Vector2Int position;
                do
                {
                    position = new Vector2Int(Random.Range(0, generationData.GenerationWidth), Random.Range(0, generationData.GenerationHeight));
                } while (TryGetTileEntityAtPosition(position) != null);

                var tileEntity = new TileEntity(position, true, false, -1f);
                _tileEntities.Add(position, tileEntity);
            }
        }

        public TileEntity TryGetTileEntityAtPosition(Vector2Int position)
        {
            TileEntity tileEntity = null;
            _tileEntities?.TryGetValue(position, out tileEntity);
            return tileEntity;
        }

        private void FixedUpdate()
        {
            if (_tileEntities != null)
            {
                var tileEntitiesCopy = _tileEntities.ToDictionary(k => k.Key, v => v.Value);
                foreach (var tileEntity in tileEntitiesCopy)
                {
                    var oldPosition = tileEntity.Key;
                    var position = tileEntity.Value.UpdateEntity();
                    if (oldPosition != position)
                    {
                        _tileEntities.Remove(oldPosition);
                        _tileEntities.Add(position, tileEntity.Value);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_tileEntities != null && _drawGizmos)
            {
                foreach (var tileEntity in _tileEntities)
                {
                    Gizmos.color = tileEntity.Value.IsSolid ? Color.black : Color.yellow;
                    Gizmos.DrawCube((Vector2)tileEntity.Key, Vector2.one);
                }
            }
        }
    }
}