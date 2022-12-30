using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Vector3 homePosition;
    [SerializeField] private Outline outline;

    [Header("Data")]
    [SerializeField] private ResourceToken token;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float initialYOffset = 5f;
    [SerializeField] private float outlineThickness = 5f;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private TextMeshProUGUI debugText;

    private void Start()
    {
        // Sub
        TokenEvents.instance.onSelect += ToggleOutline;
        TokenEvents.instance.onDestroy += DestroyToken;
    }

    private void OnDestroy()
    {
        // Unsub
        TokenEvents.instance.onSelect -= ToggleOutline;
        TokenEvents.instance.onDestroy -= DestroyToken;
    }

    public void Initialize(ResourceToken token, Transform homeTransform)
    {
        this.token = token;
        transform.position = homeTransform.position;

        DisableOutline();

        // Set home
        Relocate(homeTransform);

        // Update name
        name = this.ToString();
    }

    private void OnMouseEnter()
    {
        // EnableOutline();

        // Deselect token
        token.stack.SelectToken(token, true);
    }

    private void ToggleOutline(ResourceToken token, bool state)
    {
        // Make sure it's this token
        if (this.token != token) return;
        
        if (state)
        {
            // Highlight this token
            EnableOutline();
        }
        else 
        {
            // Remove highlight
            DisableOutline();
        }
    }

    private void OnMouseExit()
    {
        // DisableOutline();

        // Select token
        token.stack.SelectToken(token, false);

    }

    private void OnMouseOver()
    {
        // Check for Right Click
        if (Input.GetMouseButtonDown(1))
        {
            // Debug
            print("Right clicked on token: " + ToString());

            // Return token to correct stack
            var stackHandler = BoardHandler.instance.GetStackHandler();

            // Move to stack
            MoveToStack(stackHandler);
        }
    }

    private void OnMouseDown()
    {
        // Move selected tokens in the stack into another transform

        // Add to transfer
        // TransferHandler.instance.AddToTransport(transform);
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

        // Clear transport
        // TransferHandler.instance.AddToTransport(transform);
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
        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 10f, layerMask);

        // Check to see if you hit a card slot
        if (hit)
        {
            if (hitInfo.collider.transform.parent.TryGetComponent(out StackHandler stackHandler))
            {
                // Debug
                print(stackHandler.ToString());

                // Move
                MoveToStack(stackHandler);
            }
        }
    }

    private void DestroyToken(ResourceToken token)
    {
        if (this.token != token) return;

        // Destroy self
        Destroy(gameObject);
    }

    private void Relocate(Transform parent)
    {
        // Change parent
        transform.parent = parent;

        // Set home
        homePosition = parent.position;

        // Move to home
        transform.position = homePosition;
    }

    public void Return()
    {
        // TODO
        // Relocate(home);
    }

    private void MoveToStack(StackHandler stackHandler)
    {
        // Move token to new stack
        var stack = stackHandler.GetTokenStack();
        // Logic here
        var moved = token.MoveTo(stack);

        // Check if move was sucessful
        if (moved)
        {
            // Debug
            print(ToString() + " Relocated to " + stackHandler.ToString());

            // Get new location
            var newParent = stackHandler.GetTokenTransform();

            // Relocation token
            Relocate(newParent);
        }
    }

    private void EnableOutline()
    {
        outline.OutlineWidth = outlineThickness;
    }

    private void DisableOutline()
    {
        outline.OutlineWidth = 0f;
    }

    public override string ToString()
    {
        return token.ToString() + " Object";
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);

        // Set debug text
        debugText.text = "ID: " + gameObject.GetInstanceID();
        // debugText.transform .rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
