using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Vector3 homePosition;

    [Header("Data")]
    [SerializeField] private ResourceToken token;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;

    private void Start()
    {
        // Set home to where it was spawned
        homePosition = transform.position;

        // Sub
        TokenEvents.instance.onDestroy += DestroyToken;
    }

    private void OnDestroy()
    {
        // Unsub
        TokenEvents.instance.onDestroy -= DestroyToken;
    }

    public void Initialize(ResourceToken token)
    {
        this.token = token;
    }

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

        // Return card to home position
        transform.position = homePosition;
    }

    private void FollowMouse()
    {
        // Create ground plane
        Plane plane = new Plane(Vector3.up, Vector3.up * pickUpHeight);

        // Create a ray from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance)) // the distance from the ray origin to the ray intersection of the plane
        {
            transform.position = ray.GetPoint(distance); // distance along the ray
        }
    }

    private void AddToStack()
    {
        // Raycast down from the mouse
        Vector3 worldPosition = transform.position;

        // Make a raycast from the token position
        var hit = Physics.Raycast(worldPosition, Vector3.down, out RaycastHit hitInfo, 10f, layerMask);

        // Check to see if you hit something
        if (hit)
        {
            // Check to see if you hit a card
            if (hitInfo.transform.TryGetComponent(out CardHandler cardObject))
            {
                // Debug
                if (debugMode) print("Added " + name + " To " + cardObject.name + " stack.");

                // Add token to stack
                

                // Find the nearest slot to snap to
                var newPosition = cardObject.GetTokenInputPosition();

                // Set new position
                transform.position = newPosition;
            }

        }
    }

    private void DestroyToken(ResourceToken token)
    {
        if (this.token != token) return;

        // Destroy self
        Destroy(gameObject);
    }

    
    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
    }
}
