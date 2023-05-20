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
}
