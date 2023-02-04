using System;
using System.Collections.Generic;
using Klonk.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Klonk.Input
{
    [Serializable]
    public class InputKeys
    {
        public Key Left;
        public Key Up;
        public Key Right;
        public Key Down;
        public Key ZoomIn;
        public Key ZoomOut;
    }

    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private float _speed;

        [SerializeField] private List<InputKeys> _controls = new()
        {
            new InputKeys()
            {
                Left = Key.A,
                Up = Key.W,
                Right = Key.D,
                Down = Key.S,
                ZoomIn = Key.E,
                ZoomOut = Key.Q
            },
            new InputKeys()
            {
                Left = Key.LeftArrow,
                Up = Key.UpArrow,
                Right = Key.RightArrow,
                Down = Key.DownArrow,
                ZoomIn = Key.Period,
                ZoomOut = Key.Comma
            }
        };

        private WorldRenderer _worldRenderer;

        private void Start()
        {
            _worldRenderer = GetComponent<WorldRenderer>();
        }

        private void Update()
        {
            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                float scrollY = mouse.scroll.ReadValue().y;
                if (scrollY > 0)
                {
                    _worldRenderer.ZoomIn();
                }
                else if (scrollY < 0)
                {
                    _worldRenderer.ZoomOut();
                }
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            int x = 0;
            int y = 0;

            foreach (InputKeys inputKeys in _controls)
            {
                x -= keyboard[inputKeys.Left].isPressed ? 1 : 0;
                x += keyboard[inputKeys.Right].isPressed ? 1 : 0;
                y += keyboard[inputKeys.Up].isPressed ? 1 : 0;
                y -= keyboard[inputKeys.Down].isPressed ? 1 : 0;

                if (keyboard[inputKeys.ZoomIn].wasPressedThisFrame)
                {
                    _worldRenderer.ZoomIn();
                }

                if (keyboard[inputKeys.ZoomOut].wasPressedThisFrame)
                {
                    _worldRenderer.ZoomOut();
                }
            }

            if (x != 0 || y != 0)
            {
                transform.position += new Vector3(x, y, 0) * (_speed * Time.deltaTime);
            }
        }
    }
}