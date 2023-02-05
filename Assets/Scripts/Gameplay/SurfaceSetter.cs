using System;
using System.Collections;
using System.Collections.Generic;
using Klonk.TileEntity;
using UnityEngine;

namespace Klonk
{
    public class SurfaceSetter : MonoBehaviour
    {
        [SerializeField] private int _xOffset;
        [SerializeField] private int _yOffset = 1;

        [ContextMenu("Reset")]
        private void Awake()
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            
            int x = TileEntityHandler.Instance.GenerationData.GenerationWidth / 2 + _xOffset;
            int y = TileEntityHandler.Instance.GenerationData.GenerationWidth / 2 + Mathf.FloorToInt(rend.size.y) + _yOffset - TileEntityHandler.GroundStart;

            transform.position = new Vector3(x, y, 1);
        }
    }
}
