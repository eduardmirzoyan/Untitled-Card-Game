using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<TokenHandler> tokenHandlers;
    [SerializeField] private List<ResourceToken> selectedTokens;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    private StackHandler stackHandler;

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
        selectedTokens = new List<ResourceToken>();
    }

    private void Start()
    {
        TokenEvents.instance.onHover += SelectToken;
        TokenEvents.instance.onBlur += DeselectTokens;
    }

    private void OnDestroy()
    {
        TokenEvents.instance.onHover -= SelectToken;
        TokenEvents.instance.onBlur -= DeselectTokens;
    }

    private void SelectToken(ResourceToken token, bool state)
    {
        if (state)
        {
            // Add token to stack
            selectedTokens.Add(token);
        }
        else
        {
            // Remove the token
            selectedTokens.Remove(token);
        }
    }

    private void DeselectTokens(ResourceToken token)
    {
        // Clear selected tokens
        selectedTokens.Clear();
    }

    private void Update()
    {
        // If you have any tokens, then start following mouse and looking for stacks
        if (tokenHandlers.Count > 0)
        {
            // Follow the player's mouse
            FollowMouse();

            // Search for stacks
            SearchForStack();
        }
    }

    private void SearchForStack()
    {
        // Raycast down from the mouse
        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 10f, layerMask);

        // Check to see if you hit a card slot
        if (hit)
        {
            if (hitInfo.collider.transform.parent.TryGetComponent(out StackHandler stackHandler))
            {
                // Do nothing if same
                if (this.stackHandler == stackHandler) return;

                // Else check all 4 possibilities
                if (this.stackHandler != null && stackHandler != null)
                {
                    // Disable old
                    TokenEvents.instance.TriggerOnDrop(this.stackHandler.GetTokenStack(), false);

                    // Enable new
                    TokenEvents.instance.TriggerOnDrop(stackHandler.GetTokenStack(), true);
                }
                else if (this.stackHandler == null && stackHandler != null)
                {
                    // Enable new
                    TokenEvents.instance.TriggerOnDrop(stackHandler.GetTokenStack(), true);
                }
                else if (this.stackHandler != null && stackHandler == null)
                {
                    // Disable old
                    TokenEvents.instance.TriggerOnDrop(this.stackHandler.GetTokenStack(), false);
                }
                else if (this.stackHandler == null && stackHandler == null)
                {
                    // Nothing lol
                }

                // Update stack
                this.stackHandler = stackHandler;
            }
        }
    }

    public void PickupTransport(TokenHandler tokenHandler)
    {
        // Disable grav
        tokenHandler.ToggleGravity(false);

        // Set the parent to this
        tokenHandler.transform.parent = transform;

        // Calculate position
        float thickness = 0.1f;
        tokenHandler.transform.localPosition = Vector3.up * tokenHandlers.Count * thickness;

        // Save
        tokenHandlers.Add(tokenHandler);
    }

    public void GatherTransport()
    {
        // Add selected tokens into the transport
        foreach (var token in selectedTokens)
        {
            // Trigger event
            TokenEvents.instance.TriggerOnDrag(token);
        }
    }

    public void DropTransport()
    {
        // Make sure a handler was found
        if (this.stackHandler != null)
        {
            foreach (var tokenHandler in tokenHandlers)
            {
                // Enable grav
                tokenHandler.ToggleGravity(true);

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
                tokenHandler.ToggleGravity(true);

                // Return
                tokenHandler.ReturnToStack();
            }
        }

        // Now clear all handlers
        tokenHandlers.Clear();

        // Reset
        stackHandler = null;
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
}
