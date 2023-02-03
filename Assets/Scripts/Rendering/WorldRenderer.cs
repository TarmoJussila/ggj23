using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klonk.Rendering
{
    public class WorldRenderer : MonoBehaviour
    {
        private Material _material;
        private Texture2D _texture;
        private int pixelsPerTile = 32;

        void Awake()
        {
            _material = new Material(Shader.Find("Klonk/World"));
            _texture = new Texture2D(Screen.width, Screen.height);
        }

        private void FixedUpdate()
        {
            //TEMPDATA
            NewBehaviourScript[,] entities = new NewBehaviourScript[,] { };

            Vector2 cameraPos = new Vector2(500, 500);

            for (int x = 0; x < Screen.width; x++)
            {
                for (int y = 0; y < Screen.height; y++)
                {
                    var entity = entities[x, y];
                    var def = entity.GetDef();
                    _texture.SetPixel(x,y, new Color());
                }
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, _material);
        }
    }
}
