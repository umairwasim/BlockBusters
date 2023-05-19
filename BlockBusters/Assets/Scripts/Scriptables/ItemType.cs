using System;
using UnityEngine;

[Serializable]
public enum ItemObjectType
{
    Blue,
    Red,
    Green,
    Bonus,
    Explosive,
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/ItemType")]
public class ItemType : ScriptableObject
{
    public ItemObjectType itemObjectType;
    public ItemObjectType GetItemType() => itemObjectType;
    public bool isExplosiveType() => itemObjectType == ItemObjectType.Explosive;
    public bool isBonusType() => itemObjectType == ItemObjectType.Bonus;
}
