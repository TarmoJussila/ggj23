using UnityEngine;
using Klonk.TileEntity;

namespace Klonk.Rendering
{
    public class WorldRenderer : MonoBehaviour
    {
        public static WorldRenderer Instance { get; private set; }

        public Camera Camera { get; private set; }
        public int Width => _width;
        public int Height => _height;

        [SerializeField] private Material _material;
        [SerializeField] private int _width = 128;
        [SerializeField] private int _height = 72;
        [SerializeField] private Color _skyColor;

        private Texture2D _texture;
        private int _tilesPerUnit = 32;

        private void Awake()
        {
            Instance = this;

            Camera = GetComponent<Camera>();
            _texture = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
            _texture.wrapMode = TextureWrapMode.Clamp;

            _material.EnableKeyword("_WorldTex");
            _material.EnableKeyword("_CameraPos");
            _material.EnableKeyword("_WorldResolution");

            _material.SetVector("_WorldResolution", new Vector2(_width, _height));
            //_material.EnableKeyword("_MainTex");
        }

        private void FixedUpdate()
        {
            var coords = Vector2Int.zero;

            Vector3 position = transform.position;
            Vector2 uvOffset = Vector2.zero;

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
