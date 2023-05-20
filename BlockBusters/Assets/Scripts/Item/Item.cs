using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

[Serializable]
public enum ItemObjType
{
    Blue = 0,
    Red,
    Green,
    Bonus,
    Explosive,
}



public class Item : MonoBehaviour
{
    [NonSerialized]
    public CollisionPool myPool = null;

    public ItemType itemType;
    public ItemObjType itemObjectType;

    [Header("Item Properties")]
    public int baseScore;
    public int bonusMultiplier;
    public bool canInteract;
    public bool isAnimating;

    private const string DRAGABLE = "Dragable";
    private const string BONUS = " 2x";

    private void Start()
    {
        canInteract = false;
        isAnimating = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If cannot interact, return
        if (canInteract == false)
            return;

        //Check if the collision is on same Dragable layer, if not return
        if (collision.gameObject.layer != LayerMask.NameToLayer(DRAGABLE))
            return;

        //Get Collision Item
        Item collisionItem = collision.transform.GetComponent<Item>();

        //Check if pools are null, create pool and add both collided objects
        if (myPool == null)
        {
            if (collisionItem.myPool == null)
            {
                CollisionPool.CreateNewPool(this, collisionItem);
            }
            //Add my item to collided item's pool, also add the item it collided with
            else
            {
                collisionItem.myPool.CollidedItem(collisionItem);
                collisionItem.myPool.AddItem(this);
            }
            AudioManager.Instance.PlaySound(AudioManager.Instance.collisionSfx);
        }
    }

    #region Animate Group
    /// <summary>
    /// TODO: Clean code repetition 
    /// Make an Item base class and inplement logic there or some other way to remove
    /// redundant code
    /// </summary>

    public void AnimateBlocksPool(ItemCategory type)
    {
        if (isAnimating == false)
        {
            isAnimating = true;
            StopInteraction();

            switch (type)
            {
                case ItemCategory.Ordinary:
                    AnimateOrdinaryBlocks();
                    break;
                case ItemCategory.Bonus:
                    AnimateBonusBlocks();
                    break;
                case ItemCategory.Explosive:
                    AnimateExplosiveBlocks();
                    break;
                default:
                    break;
            }
        }
    }

    private void AnimateExplosiveBlocks()
    {
        transform.DOPunchScale(Vector3.one * 0.5f, 5f)
                 .OnComplete(() =>
                 {
                    //Shake camera and display Vfx
                    CameraShake.Instance.Shake(5f, 0.25f);
                     VfxManager.Instance.DisplayVfx(VfxManager.Instance.explosionVfx, transform.position);
                     AudioManager.Instance.PlaySound(AudioManager.Instance.explosionSfx);
                 });
    }

    private void AnimateBonusBlocks()
    {
        transform.DOScale(Vector3.one * 1.1f, 3f);
        AudioManager.Instance.PlaySound(AudioManager.Instance.bonusSfx);

        if (itemObjectType == ItemObjType.Bonus)
        {
            VfxManager.Instance.DisplayVfx(VfxManager.Instance.bonusVfx, transform.position);
            //Show Feedback Pop Up
            FeedbackPopUp.CreateFeedback(transform.position, BONUS);
        }
    }

    private void AnimateOrdinaryBlocks()
    {
        transform.DOShakePosition(3f, 0.1f, 10, 0);
        AudioManager.Instance.PlaySound(AudioManager.Instance.matchSfx);
        VfxManager.Instance.DisplayVfx(VfxManager.Instance.identicalVfx, transform.position);
    }


    public void AnimateSameGrouping()
    {
        if (isAnimating == false)
        {
            isAnimating = true;
            StopInteraction();
            transform.DOShakePosition(3f, 0.1f, 10, 0);
            AudioManager.Instance.PlaySound(AudioManager.Instance.matchSfx);
            VfxManager.Instance.DisplayVfx(VfxManager.Instance.identicalVfx, transform.position);
        }
    }

    public void AnimateExplosiveGrouping()
    {
        if (isAnimating == false)
        {
            isAnimating = true;
            StopInteraction();
            transform.DOPunchScale(Vector3.one * 0.5f, 5f)
                .OnComplete(() =>
                {
                    //Shake camera and display Vfx
                    CameraShake.Instance.Shake(5f, 0.25f);
                    VfxManager.Instance.DisplayVfx(VfxManager.Instance.explosionVfx, transform.position);
                    AudioManager.Instance.PlaySound(AudioManager.Instance.explosionSfx);
                });
        }
    }

    public void AnimateBonusGrouping()
    {
        if (isAnimating == false)
        {
            isAnimating = true;
            StopInteraction();
            transform.DOScale(Vector3.one * 1.1f, 3f);
            AudioManager.Instance.PlaySound(AudioManager.Instance.bonusSfx);

            if (itemObjectType == ItemObjType.Bonus)
            {
                VfxManager.Instance.DisplayVfx(VfxManager.Instance.bonusVfx, transform.position);
                //Show Feedback Pop Up
                FeedbackPopUp.CreateFeedback(transform.position, BONUS);
            }
        }
    }

    public void FossilizeGrouping(Color fossilizeColor)
    {
        StopInteraction();
        GetComponent<Renderer>().material.color = fossilizeColor;
        AudioManager.Instance.PlaySound(AudioManager.Instance.fossilizeSfx);
        VfxManager.Instance.DisplayVfx(VfxManager.Instance.fossilizeVfx, transform.position);
    }

    #endregion

    //Stop Item interaction so that it cannot interact further
    public void StopInteraction()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<TouchAndDrag>().canDrag = false;
    }

    #region Disable Item

    public void DisableItem(float delay)
    {
        StartCoroutine(DisableItemRoutine(delay));
    }

    public IEnumerator DisableItemRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject parentObject = transform.parent.gameObject;

        //Remove the item's gameObject form the list and destroy afterward
        BlocksManager.Instance.RemoveItem(parentObject);

        transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Destroy(parentObject));
    }

    #endregion
}

