using System;
using Klonk.TileEntity;
using UnityEngine;

namespace Klonk.Platforming
{
    [RequireComponent(typeof(BoxCollider2D))] 
    public class FakeRigidbody : MonoBehaviour
    {
        [SerializeField] private bool _checkLeft = true; 
        
        public Vector2 Velocity => _velocity;
        public bool IsGrounded { get; private set; }

        private Vector2 _velocity;
        private BoxCollider2D _boxCollider;
        private const float GRAVITY = 0.25f;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate()
        {
            ClampVelocity();
            Vector3 position = transform.position;
            position += new Vector3(_velocity.x, _velocity.y, default);
            transform.position = position;
        }

        private void ClampVelocity()
        {
            _velocity.y -= GRAVITY;
            
            Bounds bounds = _boxCollider.bounds;
            Vector3 worldPosition = transform.position;
            Vector3 worldPositionAfterVelocity = worldPosition + new Vector3(_velocity.x, _velocity.y, default);
            int yTileVelocity = (int)(_velocity.y * TileUtility.TILES_PER_UNIT);
            int xTileVelocity = (int)(_velocity.x * TileUtility.TILES_PER_UNIT);
            if (_velocity.x < 0 - Mathf.Epsilon && xTileVelocity == default)
            {
                xTileVelocity = -1;
            }
            
            if (_velocity.x > Mathf.Epsilon && xTileVelocity == default)
            {
                xTileVelocity = 1;
            }
            
            if (_velocity.y < 0 - Mathf.Epsilon && yTileVelocity == default)
            {
                yTileVelocity = -1;
            }
            
            if (_velocity.y > Mathf.Epsilon && yTileVelocity == default)
            {
                yTileVelocity = 1;
            }
            
            int xTileSize = (int)(bounds.size.x * TileUtility.TILES_PER_UNIT);
            int yTileSize = (int)(bounds.size.y * TileUtility.TILES_PER_UNIT);
            Vector3 bottomLeft = worldPosition + Vector3.down * bounds.extents.y + Vector3.left * bounds.extents.x;
            Vector3 bottomRight = worldPosition + Vector3.down * bounds.extents.y + Vector3.right * bounds.extents.x;
            Vector3 topLeft = worldPosition + Vector3.up * bounds.extents.y + Vector3.left * bounds.extents.x;
            Vector2Int bottomLeftTileCoordinates = TileUtility.WorldToTileCoordinates(bottomLeft);
            Vector2Int bottomRightTileCoordinates = TileUtility.WorldToTileCoordinates(bottomRight);
            Vector2Int topRightTileCoordinates = TileUtility.WorldToTileCoordinates(topLeft);
            
            // Limit down velocity
            for (int x = 0; x < xTileSize; x++)
            {
                for (int y = 0; y > yTileVelocity - 1; y--)
                {
                    Vector2Int position = bottomLeftTileCoordinates + new Vector2Int(x + 1, y - 1);
                    position.y = Mathf.Max(default, position.y);
                    if (TileEntityHandler.Instance.TryGetTileEntityAtPosition(position, out _))
                    {
                        _velocity = new Vector2(_velocity.x, y * TileUtility.TILE_SIZE);
                        IsGrounded = Mathf.Approximately(_velocity.y, default);
                        break;
                    }
                }
            }
            
            // Limit up velocity
            for (int x = 0; x < xTileSize; x++)
            {
                for (int y = 0; y < yTileVelocity; y++)
                {
                    Vector2Int position = topRightTileCoordinates + new Vector2Int(x + 1, y + 1);
                    position.y = Mathf.Max(default, position.y);
                    if (TileEntityHandler.Instance.TryGetTileEntityAtPosition(position, out _))
                    {
                        _velocity = new Vector2(_velocity.x, Mathf.Min(_velocity.y, y * TileUtility.TILE_SIZE));
                        break;
                    }
                }
            }
            
            // Limit left velocity
            for (int y = 1; y < yTileSize; y++)
            {
                for (int x = 0; x > xTileVelocity - 1; x--)
                {
                    Vector2Int position = bottomLeftTileCoordinates + new Vector2Int(x - 1, y - 1);
                    if (TileEntityHandler.Instance.TryGetTileEntityAtPosition(position, out _))
                    {
                        _velocity = new Vector2(Mathf.Max(x * TileUtility.TILE_SIZE, _velocity.x), _velocity.y);
                        break;
                    }
                }
            }
            
            // Limit right velocity
            for (int y = 1; y < yTileSize; y++)
            {
                for (int x = 0; x < xTileVelocity; x++)
                {
                    Vector2Int position = bottomRightTileCoordinates + new Vector2Int(x + 1, y - 1);
                    if (TileEntityHandler.Instance.TryGetTileEntityAtPosition(position, out _))
                    {
                        _velocity = new Vector2(Mathf.Min(x * TileUtility.TILE_SIZE, _velocity.x), _velocity.y);
                        break;
                    }
                }
            }

            if (transform.position.y + _velocity.y < 0)
            {
                _velocity.y = -transform.position.y;
            }
        }

        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
        }
    }
}
