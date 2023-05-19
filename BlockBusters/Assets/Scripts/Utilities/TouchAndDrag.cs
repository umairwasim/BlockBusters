using UnityEngine;

public class TouchAndDrag : MonoBehaviour
{
    public bool canDrag;

    [SerializeField] LayerMask layerMask;

    private Item item;
    private bool isEnabled = false;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    private void Start()
    {
        canDrag = true;
    }

    private void OnMouseDrag()
    {
        //If it is dragable
        if (canDrag == true)
        {
            //Shoot a ray from screen point in the direction of mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if it hits an item
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                //place that item to the hit point
                transform.position = raycastHit.point;

                //enable the component
                if (isEnabled == false)
                {
                    isEnabled = true;
                    item.canInteract = true;
                }
            }
        }
    }

    private void OnMouseUp()
    {
        //disable the component
        if (isEnabled == true)
        {
            isEnabled = false;
            item.canInteract = false;
        }
    }
}

//    #region Old Implementaton 

//    private Vector3 screenPoint;
//    private Vector3 offset;

//    private void OnMouseDown()
//    {
//        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
//        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3
//            (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
//    }

//    private void OnMouseDrag()
//    {
//        Vector3 currentScreenPoint = new(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
//        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;
//        transform.position = currentPosition;
//    }
//}

//#endregion