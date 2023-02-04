using UnityEngine;
using UnityEngine.InputSystem;

namespace Klonk
{
    public class MoveCamera : MonoBehaviour
    {
        public float Speed;
        public Key LeftKey = Key.A;
        public Key UpKey = Key.W;
        public Key RightKey = Key.D;
        public Key DownKey = Key.S;

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            int x = 0;
            int y = 0;
            x -= keyboard[LeftKey].isPressed ? 1 : 0;
            x += keyboard[RightKey].isPressed ? 1 : 0;
            y += keyboard[UpKey].isPressed ? 1 : 0;
            y -= keyboard[DownKey].isPressed ? 1 : 0;

            if (x != 0 || y != 0)
            {
                transform.position += new Vector3(x, y, 0) * Speed;
            }
        }
    }
}
