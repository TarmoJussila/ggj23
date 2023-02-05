using System;
using System.Collections;
using System.Collections.Generic;
using Klonk.Platforming;
using Klonk.TileEntity;
using Unity.Mathematics;
using UnityEngine;

namespace Klonk
{
    public class OverlapChecker : MonoBehaviour
    {
        private FakeRigidbody _fakeRigidbody;
        private Bounds _bounds;
        private Vector3 _topLeftOffset;

        [SerializeField] private int _solidOverlapDamageThreshold = 6;

        [HideInInspector] public List<LiquidType> OverlapLiquids = new();
        [HideInInspector] public List<SolidType> OverlapSolids = new();

        private void Start()
        {
            _fakeRigidbody = GetComponent<FakeRigidbody>();
            _bounds = _fakeRigidbody.BoxCollider.bounds;
            _topLeftOffset = Vector3.up * _bounds.extents.y + Vector3.left * _bounds.extents.x;
        }

        private void FixedUpdate()
        {
            Vector3 topLeft = _fakeRigidbody.RigidBodyPosition + _topLeftOffset;
            
            OverlapLiquids = new();
            OverlapSolids = new();

            for (int x = 0; x < _bounds.size.x; x++)
            {
                for (int y = 0; y < _bounds.size.y; y++)
                {
                    Vector2Int coords = new Vector2Int(Mathf.FloorToInt(topLeft.x + x), Mathf.FloorToInt(topLeft.y - y));

                    if (!TileEntityHandler.Instance.TryGetTileEntityAtPosition(coords, out var tile))
                    {
                        continue;
                    }

                    if (tile.IsLiquid)
                    {
                        OverlapLiquids.Add(tile.LiquidType);
                    }

                    if (tile.IsSolid)
                    {
                        OverlapSolids.Add(tile.SolidType);
                    }
                }
            }
        }
    }
}
