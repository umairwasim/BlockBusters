using System;
using UnityEngine;

[Serializable]
public enum ItemCategory
{
    Ordinary,
    Bonus,
    Explosive,
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/ItemType")]
public class ItemType : ScriptableObject
{
    public ItemCategory itemCategoryType;
    public ItemCategory GetItemType() => itemCategoryType;

    public bool isOrdinaryType() => itemCategoryType == ItemCategory.Ordinary;
    public bool isExplosiveType() => itemCategoryType == ItemCategory.Explosive;
    public bool isBonusType() => itemCategoryType == ItemCategory.Bonus;
}
