using System;
using UnityEngine;
using Klonk.TileEntity;
using UnityEngine.Rendering;

namespace Klonk.Rendering
{
    public class WorldRenderer : MonoBehaviour
    {
        public static WorldRenderer Instance { get; private set; }

        public Camera Camera { get; private set; }
        public int Width => _width;
        public int Height => _height;

        [SerializeField] private Material _material;
        [SerializeField] public int TextureResDivider { get; private set; } = 10;
        [SerializeField] private Color _skyColor;
        [SerializeField] private Camera _normalCamera;

        private Texture2D _texture;
        private int _tilesPerUnit = 32;

        private int _lastScreenWidth, _lastScreenHeight;
        private int _width, _height;
        private readonly int _minDivider = 4;
        private readonly int _maxDivider = 12;
        
        private void Awake()
        {
            Instance = this;

            Camera = GetComponent<Camera>();

            _material.EnableKeyword("_WorldTex");
            _material.EnableKeyword("_CameraPos");
            _material.EnableKeyword("_WorldResolution");

            ResetTexture();
        }

        private void FixedUpdate()
        {
            var coords = Vector2Int.zero;

            Vector3 position = transform.position;
            Vector2 uvOffset = Vector2.zero;

            _normalCamera.transform.position = position + new Vector3(position.x + Width / 2f, position.y + Height / 2f, _normalCamera.transform.position.z);
            _normalCamera.orthographicSize = Mathf.Max(Width / 2f, Height / 2f);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    coords.x = Mathf.FloorToInt(position.x) + x;
                    coords.y = Mathf.FloorToInt(position.y) + y;

                    if (!TileEntityHandler.Instance.TryGetTileEntityAtPosition(coords.x, coords.y, out TileEntity.TileEntity tile))
                    {
                        _texture.SetPixel(x, y, _skyColor);
                        continue;
                    }

                    //uvOffset.x = tile.IsLiquid ? 0.25f : 0.0f;
                    //uvOffset.y = 0.75f; //tile.IsLiquid ? 0.0f : 0.0f;

                    _texture.SetPixel(x, y, tile.TileColor);
                    //_texture.SetPixel(x, y, new Color(uvOffset.x, uvOffset.y, 0.0f, 1.0f));
                }
            }

            _texture.Apply();

            _material.SetTexture("_WorldTex", _texture);
            _material.SetVector("_CameraPos", position);
        }

        private void Update()
        {
            CheckAspect();
        }

        private void CheckAspect(bool force = false)
        {
            if (_lastScreenWidth != Screen.width || _lastScreenHeight != Screen.height || force)
            {
                _lastScreenWidth = Screen.width;
                _lastScreenHeight = Screen.height;
                _width = Screen.width / TextureResDivider;
                _height = Screen.height / TextureResDivider;

                ResetTexture();
            }
        }

        [ContextMenu("Zoom In")]
        public void ZoomIn()
        {
            ChangeZoom(3);
        }

        [ContextMenu("Zoom Out")]
        public void ZoomOut()
        {
            ChangeZoom(-3);
        }

        private void ChangeZoom(int direction)
        {
            TextureResDivider = Math.Clamp(TextureResDivider + direction, _minDivider, _maxDivider);
            CheckAspect(true);
        }

        private void ResetTexture()
        {
            _texture = new Texture2D(_width, _height, TextureFormat.ARGB32, false);
            _texture.wrapMode = TextureWrapMode.Clamp;

            _material.SetVector("_WorldResolution", new Vector2(_width, _height));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, _material);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(transform.position.x + _width / 2, transform.position.y + _height / 2), new Vector2(_width, _height));
        }
    }
}
