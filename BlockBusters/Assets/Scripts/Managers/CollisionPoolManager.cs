using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionPoolManager : MonoBehaviour
{
    public static CollisionPoolManager Instance;

    public List<CollisionPool> collisionPools = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddToCollisionPools(CollisionPool collisionPool)
    {
        collisionPools.Add(collisionPool);
    }

    public void RemoveFromCollisionPools(CollisionPool collisionPool)
    {
        collisionPools.Remove(collisionPool);
    }

    private void Update()
    {
        for (int i = 0; i < collisionPools.Count; i++)
        {
            collisionPools[i].UpdateTimer();
        }
    }
}

[System.Serializable]
public class CollisionPool : IDisposable
{
    //List of collided items 
    public List<Item> collidedList;
    public float timer;
    public ItemObjType myObjectType;

    private Item collidedItem;
    private int score;
    private int lifeLost;
    private int baseScore;
    private int bonusMultipier;
    private bool isEnteredOnce = false;
    private const float totatlTime = 5f;

    public static void CreateNewPool(Item myItem, Item collidedItem)
    {
        CollisionPool collisionPool = new();
        collisionPool.collidedList = new List<Item>();
        collisionPool.AddItem(myItem);
        collisionPool.CollidedItem(myItem);
        collisionPool.AddItem(collidedItem);

        CollisionPoolManager.Instance.AddToCollisionPools(collisionPool);
    }

    public void CollidedItem(Item item)
    {
        collidedItem = item;
    }

    public void AddItem(Item item)
    {
        //make this item part of the pool it is added to
        item.myPool = this;
        baseScore = item.baseScore;
        bonusMultipier = item.bonusMultiplier;

        collidedList.Add(item);

        //check for multiple items
        if (collidedList.Count > 1)
        {
            if (item.itemObjectType == collidedItem.itemObjectType)
            {
                AnimatePoolBlocks(ItemCategory.Ordinary);
                // AnimateSameGroup();
                return;
            }
            //if different objects are collided 
            else if (item.itemObjectType != collidedItem.itemObjectType)
            {
                //if object is explosive item
                if (item.itemObjectType == ItemObjType.Explosive ||
                    collidedItem.itemObjectType == ItemObjType.Explosive)
                {
                    AnimatePoolBlocks(ItemCategory.Explosive);
                    //AnimateExplosiveGroup();
                    return;
                }
                else if (item.itemObjectType == ItemObjType.Bonus ||
                    collidedItem.itemObjectType == ItemObjType.Bonus)
                {
                    AnimatePoolBlocks(ItemCategory.Bonus);
                    //AnimateBonusGroup();
                    return;
                }
                else
                {
                    FossilizeGroup();
                    return;
                }
            }
        }
        ResetTimer();
    }

    #region Animate Group

    private void AnimatePoolBlocks(ItemCategory itemCategory)
    {
        for (int i = 0; i < collidedList.Count; i++)
        {
            if (!collidedList[i].isAnimating)
            {
                collidedList[i].AnimateBlocksPool(itemCategory);
            }
        }
    }

    private void AnimateBonusGroup()
    {
        for (int i = 0; i < collidedList.Count; i++)
        {
            if (!collidedList[i].isAnimating)
            {
                collidedList[i].AnimateBonusGrouping();
                score += baseScore * bonusMultipier;
            }
        }
    }

    private void AnimateExplosiveGroup()
    {
        for (int i = 0; i < collidedList.Count; i++)
        {
            if (!collidedList[i].isAnimating)
            {
                collidedList[i].AnimateExplosiveGrouping();
                //score += baseScore * bonusMultipier;
            }
        }
    }

    private void FossilizeGroup()
    {
        for (int i = 0; i < collidedList.Count; i++)
        {
            collidedList[i].FossilizeGrouping(Color.gray);
        }
        lifeLost = 1;
    }

    private void AnimateSameGroup()
    {
        for (int i = 0; i < collidedList.Count; i++)
        {
            if (!collidedList[i].isAnimating)
            {
                collidedList[i].AnimateSameGrouping();
                score += baseScore * bonusMultipier;
            }
        }
    }

    #endregion

    public void UpdateTimer()
    {
        if (timer <= 0 && isEnteredOnce == false)
        {
            isEnteredOnce = true;
            ExecutePool();
        }
        timer -= Time.deltaTime;
    }

    public void ResetTimer()
    {
        timer = totatlTime;
    }

    public void ExecutePool()
    {
        //Update UI of Score and Life
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateLife(lifeLost);

        //Loop through all the items in the list
        for (int i = 0; i < collidedList.Count; i++)
        {
            //Disable that item
            collidedList[i].DisableItem(0.01f);
        }

        //Remove this Pool form Collision Pools list and dispose
        CollisionPoolManager.Instance.RemoveFromCollisionPools(this);
        Dispose();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

