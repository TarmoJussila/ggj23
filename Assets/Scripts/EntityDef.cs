using System.Numerics;
using UnityEngine;

namespace Klonk
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityDef", order = 1)]
    public class EntityDef : ScriptableObject
    {
        public int Id;

        public int TextureIndex;
        public UnityEngine.Vector2 UvMin;
        public UnityEngine.Vector2 UvMax;
    }
    
}
