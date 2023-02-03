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
        [SerializeField] private Texture2D _worldTexture;

        private Material _material;
        private Texture2D _texture;
        private int _tilesPerUnit = 32;
        private Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
            _material = new Material(Shader.Find("Klonk/World"));
            _texture = new Texture2D(Screen.width, Screen.height);

            _material.SetTexture("Texture", _worldTexture);
        }

        private void FixedUpdate()
        {
            var data = TileEntityHandler.Instance.TileEntities;

            Color[] colors = new Color[Screen.width * Screen.height];
            var coords = Vector2Int.zero;
            int radiusX = Screen.width / 20;
            int radiusY = Screen.width / 20;

            for (int x = -radiusX; x < radiusX; x++)
            {
                for (int y = -radiusY; y < radiusY; y++)
                {
                    coords.x = x;
                    coords.y = y;

                    //Vector3 entityPoint = screenToWorld * _tilesPerUnit;

                    /*Vector2 entityPoint = cameraPos;
                    entityPoint.x -= halfWidth;
                    entityPoint.y -= halfHeight;

                    entityX = entityPoint.x / _tilesPerUnit;*/

                    /*Vector3 screenToWorld = Vector3.zero; // _camera.ScreenToWorldPoint(new Vector3(x, y, 0));
                    Vector2 entityPoint = Vector2.zero;
                    entityPoint.x = screenToWorld.x * _tilesPerUnit;
                    entityPoint.y = screenToWorld.y * _tilesPerUnit;

                    float entityTX = entityPoint.x % 1;
                    float entityTY = entityPoint.y % 1;*/

                    var entity = TileEntityHandler.Instance.TryGetTileEntityAtPosition(coords);
                    if (entity == null)
                    {
                        _texture.SetPixel(x,y, new Color(0f, 0f, 0.0f, 1.0f));
                        continue;
                    }

                    //var def = entity.EntityDefinition;
                    //float UvX = Mathf.Lerp(def.UvMin.x, def.UvMax.x, entityTX);
                    //float UvY = Mathf.Lerp(def.UvMin.y, def.UvMax.y, entityTY);

                    float UvX = 0.0f; //Mathf.Lerp(def.UvMin.x, def.UvMax.x, entityTX);
                    float UvY = 0.0f; //Mathf.Lerp(def.UvMin.y, def.UvMax.y, entityTY);
                    //Vector2 UV = new Vector2(UvX, UvY);

                    _texture.SetPixel(x,y, new Color(0f, 0f, 0.0f, 1.0f));
                }
            }

            _texture.SetPixels(colors);

            _material.SetTexture("WorldData", _texture);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, _material);
        }
    }
}
