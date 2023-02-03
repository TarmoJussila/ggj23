using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityDef", order = 1)]
public class EntityDef : ScriptableObject
{
    public int Id;

    public int TextureIndex;
}
