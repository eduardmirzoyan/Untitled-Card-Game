using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    [Header("Components")]
    // TODO

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    private void OnMouseDown()
    {

    }

    private void OnMouseDrag()
    {
        FollowMouse();
    }

    private void OnMouseUp()
    {
        // The card is dropped and should snap to a grid
        SnapToGrid();
    }

    private void FollowMouse()
    {
        // Create ground plane
        Plane plane = new Plane(Vector3.up, Vector3.up * pickUpHeight);

        // Create a ray from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance; // the distance from the ray origin to the ray intersection of the plane
        if (plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance); // distance along the ray
        }
    }

    private void SnapToGrid()
    {
        // Raycast down from the mouse
        Vector3 worldPos = transform.position;
        RaycastHit hitInfo;
        
        var hit = Physics.Raycast(worldPos, Vector3.down, out hitInfo, 10f, layerMask);

        // Check to see if you hit a grid
        if (hit)
        {
            if (hitInfo.transform.TryGetComponent(out BoardObject boardObject)) {
                // Debug
                //print(worldPos);

                // Find the nearest slot to snap to
                var boardPosition = boardObject.GetNearestGridPosition(worldPos);

                // Set new position
                transform.position = boardPosition;

            }
                

        }
    }

    private void OnDrawGizmos()
    {
        // print("Drawing gizmos");
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
    }
}
