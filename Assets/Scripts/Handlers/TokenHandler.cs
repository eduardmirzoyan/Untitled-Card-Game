using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenHandler : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ResourceToken token;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;

    private void OnMouseDown()
    {

    }

    private void OnMouseDrag()
    {
        FollowMouse();
    }

    private void OnMouseUp()
    {
        
        // Token should check to see if there is a stack it can add itself to
        AddToStack();
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

    private void AddToStack()
    {
        // Raycast down from the mouse
        Vector3 worldPos = transform.position;
        RaycastHit hitInfo;

        var hit = Physics.Raycast(worldPos, Vector3.down, out hitInfo, 10f, layerMask);

        // Check to see if you hit something
        if (hit)
        {
            // Check to see if you hit a card
            if (hitInfo.transform.TryGetComponent(out CardHandler cardObject))
            {
                // Debug
                if (debugMode) print("Added " + name + " To " + cardObject.name + " stack.");

                // Find the nearest slot to snap to
                var newPosition = cardObject.GetTokenInputPosition();

                // Set new position
                transform.position = newPosition;
            }

        }
    }

    
    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
    }
}
