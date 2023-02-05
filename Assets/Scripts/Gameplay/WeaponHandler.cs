using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Klonk.Platforming;
using Klonk.TileEntity;
using Klonk.TileEntity.Data;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Klonk
{
    [Serializable]
    public class Weapon
    {
        public int TileDataIndex;
        public bool RequireKeyDown;
        public string Name;
        public Sprite Sprite;
    }

    public class WeaponHandler : MonoBehaviour
    {
        public static WeaponHandler Instance;

        public static event System.Action<Weapon> OnWeaponChange;

        [SerializeField] private Weapon[] _weapons;

        private int _currentWeaponIndex;
        private Weapon _currentWeapon;

        private void Awake()
        {
            Instance = this;
            SetWeapon(0);
        }

        private void Update()
        {
            if ((UnityEngine.Input.GetKey(KeyCode.LeftShift) && !_currentWeapon.RequireKeyDown)
                || (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) && _currentWeapon.RequireKeyDown))
            {
                UseWeapon();
            }

            for (int i = 1; i <= 9; i++)
            {
                if (UnityEngine.Input.GetKeyDown(i.ToString()))
                {
                    SetWeapon(i - 1);
                }
            }
        }

        public void SetWeapon(int index)
        {
            if (index < 0 || index >= _weapons.Length)
            {
                return;
            }
            _currentWeaponIndex = index;
            _currentWeapon = _weapons[_currentWeaponIndex];
            OnWeaponChange?.Invoke(_currentWeapon);
        }

        private void UseWeapon()
        {
            int direction = PlayerMovement.Instance.GetHorizontalDirection();

            Vector3 size = PlayerMovement.Instance.Rigidbody.BoxCollider.size;
            Vector3 charPos = PlayerMovement.Instance.transform.position;
            int safeX = Mathf.FloorToInt(charPos.x + (direction * ((size.x + 4) / 2)));
            int safeY = Mathf.FloorToInt(charPos.y) - 2; //Mathf.FloorToInt(charPos.y + (direction.y * ((size.y + 2) / 2)));

            if (!TileEntityHandler.Instance.IsInBounds(safeX, safeY))
            {
                return;
            }

            if (TileEntityHandler.Instance.TryGetTileEntityAtPosition(safeX, safeY, out _))
            {
                return;
            }

            Vector2Int pos = new(safeX, safeY);

            TileData tileToSpawn = TileEntityHandler.Instance.EntityData.GetTileDataByIndex(_currentWeapon.TileDataIndex);

            var tileEntity = new Klonk.TileEntity.TileEntity(
                pos,
                tileToSpawn.LiquidType,
                tileToSpawn.SolidType,
                tileToSpawn.ExplosionType,
                tileToSpawn.IsSpawnSource,
                direction
            );

            TileEntityHandler.Instance.TryAddTileToPosition(tileEntity, pos);
        }
    }
}
