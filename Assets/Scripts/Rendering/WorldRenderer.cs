using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klonk.TileEntity;
using Unity.VisualScripting;

namespace Klonk.Rendering
{
    public class WorldRenderer : MonoBehaviour
    {
        public static WorldRenderer Instance = null;
        
        [SerializeField] private Texture2D _worldTexture;

        [SerializeField] private Material _material;
        private Texture2D _texture;
        private int _tilesPerUnit = 32;
        public Camera Cam;

        private int width;
        private int height;

        void Awake()
        {
            Instance = this;
            
            width = 128;
            height = 72;

            Cam = GetComponent<Camera>();
            _texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            _material.EnableKeyword("_WorldTex");
        }

        private void FixedUpdate()
        {
            var data = TileEntityHandler.Instance.TileEntities;

            var coords = Vector2Int.zero;

            Vector3 position = transform.position;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    coords.x = Mathf.FloorToInt(position.x) + x;
                    coords.y = Mathf.FloorToInt(position.y) + y;

                    var entity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(coords);
                    if (entity == null)
                    {
                        _texture.SetPixel(x, y, new Color(0.5f, 0.5f, 0.5f, 1.0f));
                        continue;
                    }

                    var def = entity.EntityDefinition;

                    float UvX = 0.0f;
                    float UvY = 0.0f;
                    

                    _texture.SetPixel(x, y, entity.IsLiquid ? Color.yellow : Color.black);
                }
            }

            _texture.Apply();

            _material.SetTexture("_WorldTex", _texture);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, _material);
        }
    }
}
