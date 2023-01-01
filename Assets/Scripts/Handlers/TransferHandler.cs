using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<TokenHandler> tokenHandlers;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    private StackHandler stackHandler;
    private bool isPickedUp;

    public static TransferHandler instance;
    private void Awake()
    {
        // Singleton Logic
        if (TransferHandler.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Init lists
        tokenHandlers = new List<TokenHandler>();
    }

    public void SelectToken(TokenHandler tokenHandler)
    {
        // Don't do anything if picked up
        if (isPickedUp) return;

        // Add token to list to move
        tokenHandlers.Add(tokenHandler);
    }

    public void DeselectToken(TokenHandler tokenHandler)
    {
        // Don't do anything if picked up
        if (isPickedUp) return;

        // Remove the token
        tokenHandlers.Remove(tokenHandler);
    }

    public void ClearAllTokens()
    {
        // Clear selected tokens
        tokenHandlers.Clear();
    }

    public void PickupTokens()
    {
        // // Debug
        // print("PICKED UP!");

        for (int i = 0; i < tokenHandlers.Count; i++)
        {
            // Disable gravity
            tokenHandlers[i].EnablePhysics(false);

            // Set the parent to this
            tokenHandlers[i].transform.parent = transform;

            // Calculate position
            float thickness = 0.1f;
            tokenHandlers[i].transform.localPosition = Vector3.up * i * thickness;
        }
        
        // Set current stack to null
        stackHandler = null;

        // Set state
        isPickedUp = true;
    }

    public void DropTokens()
    {
        // // Debug
        // print("DROPPED!");

        // Make sure a handler was found
        if (this.stackHandler != null)
        {
            foreach (var tokenHandler in tokenHandlers)
            {
                // Enable grav
                tokenHandler.EnablePhysics(true);

                // Move each token to stack
                tokenHandler.MoveToStack(stackHandler);
            }
        }
        else
        {
            // Return tokens back to their home
            foreach (var tokenHandler in tokenHandlers)
            {
                // Enable grav
                tokenHandler.EnablePhysics(true);

                // Return
                tokenHandler.ReturnToStack();
            }
        }

        // Now clear all handlers
        tokenHandlers.Clear();

        // Reset stack
        stackHandler = null;

        // Set state
        isPickedUp = false;
    }

    private void Update()
    {
        // If you have any tokens, then start following mouse and looking for stacks
        if (isPickedUp)
        {
            // Follow the player's mouse
            FollowMouse();

            // Search for stacks
            SearchForStack();
        }
    }

    private void SearchForStack()
    {
        // Create a ray from mouse position towards the board
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, layerMask);

        // Check to see if you hit a new stack
        if (hit && hitInfo.collider.transform.parent.TryGetComponent(out StackHandler stackHandler))
        {
            // If same, then dip
            if (this.stackHandler == stackHandler) return;

            // If a a previous stack exists
            if (this.stackHandler != null)
            {
                // Disable its highlight
                this.stackHandler.DisableHighlight();
            }
            // No previous stack
            else 
            {
                // Do nothing
            }

            // Enable new highlight
            stackHandler.EnableHighlight();

            // Save handler
            this.stackHandler = stackHandler;
        }
        // No new stack, so clear handler if possible
        else { 
            // If you previously had one
            if (this.stackHandler != null)
            {
                // Disable its highlight
                this.stackHandler.DisableHighlight();

                // Set it to null
                this.stackHandler = null;
            }
        }
    }

    private void FollowMouse()
    {
        // Create ground plane
        Plane plane = new Plane(Vector3.up, Vector3.up * pickUpHeight);

        // Create a ray from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance)) // the distance from the ray origin to the ray intersection of the plane
        {
            // Translate this object
            transform.position = ray.GetPoint(distance); // distance along the ray
        }
    }
}
