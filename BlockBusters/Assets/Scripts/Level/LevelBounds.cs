using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    public static LevelBounds Instance;

    private const string DRAGABLE = "Dragable";
  
    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Only check for shapes layer
        if (other.gameObject.layer != LayerMask.NameToLayer(DRAGABLE))
            return;

        //check if it has item script, destroy it
        if (other.TryGetComponent(out Item item))
        {
            item.StopInteraction();
            item.DisableItem();
            VfxManager.Instance.DisplayVfx(VfxManager.Instance.outOfBoundsVfx, item.transform.position);
            AudioManager.Instance.PlaySound(AudioManager.Instance.outOfBoundaSfx);
        }
    }
}
