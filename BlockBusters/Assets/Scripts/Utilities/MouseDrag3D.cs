using UnityEngine;

public class MouseDrag3D : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point;
        }
    }

}
