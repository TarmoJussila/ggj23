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

        private void Start()
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            
            int x = TileEntityHandler.Instance.GenerationData.GenerationWidth / 2 + _xOffset;
            int y = TileEntityHandler.Instance.GenerationData.GenerationWidth / 2 + Mathf.FloorToInt(rend.size.y) + _yOffset;

            transform.position = new Vector3(x, y, 0);
        }
    }
}
