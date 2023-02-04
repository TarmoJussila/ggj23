using System;
using System.Collections.Generic;
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
    }
    
    public class MoveCamera : MonoBehaviour
    {
        
        [SerializeField] private float _speed;

        [SerializeField] private List<InputKeys> _controls = new() {
            new InputKeys()
            {
                Left = Key.A,
                Up = Key.W,
                Right = Key.D,
                Down = Key.S
            },
            new InputKeys()
            {
                Left = Key.LeftArrow,
                Up = Key.UpArrow,
                Right = Key.RightArrow,
                Down = Key.DownArrow
            }
        };

        private void Update()
        {
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
            }

            if (x != 0 || y != 0)
            {
                transform.position += new Vector3(x, y, 0) * (_speed * Time.deltaTime);
            }
        }
    }
}
