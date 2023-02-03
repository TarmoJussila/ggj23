using System.Collections.Generic;
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
        
        private Dictionary<Vector2Int, TileEntity> _tileEntities;

        [SerializeField] private TileEntityGenerationData _generationData;

        private void Awake()
        {
            _tileEntities = new Dictionary<Vector2Int, TileEntity>();
            GenerateTileEntities(_generationData);
        }

        private void GenerateTileEntities(TileEntityGenerationData generationData)
        {
            for (int i = 0; i < generationData.GenerationAmount; i++)
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
            foreach (var tileEntity in _tileEntities)
            {
                tileEntity.Value.UpdateEntity();
            }
        }
    }
}