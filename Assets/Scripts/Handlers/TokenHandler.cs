using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using cakeslice;

public class TokenHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Vector3 homePosition;
    [SerializeField] private Outline outline;
    [SerializeField] private Rigidbody body;
    [SerializeField] private MeshRenderer meshRenderer;

    [Header("Data")]
    [SerializeField] private ResourceToken token;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private TextMeshProUGUI debugText;

    private StackHandler stackHandler;

    private void Start()
    {
        // Sub
        TokenEvents.instance.onHover += OnHover;
        TokenEvents.instance.onDrag += OnDrag;
        TokenEvents.instance.onDrop += OnDrop;
        TokenEvents.instance.onBlur += OnBlur;


        TokenEvents.instance.onDestroy += DestroyToken;
    }

    private void OnDestroy()
    {
        // Unsub
        TokenEvents.instance.onHover -= OnHover;
        TokenEvents.instance.onDrag -= OnDrag;
        TokenEvents.instance.onDrop -= OnDrop;
        TokenEvents.instance.onBlur -= OnBlur;

        TokenEvents.instance.onDestroy -= DestroyToken;
    }

    public void Initialize(ResourceToken token, StackHandler stackHandler)
    {
        this.token = token;
        this.stackHandler = stackHandler;

        // Disable outline
        DisableOutline();

        // Set home
        Relocate(stackHandler);

        // Generate random color
        meshRenderer.material.color = UnityEngine.Random.ColorHSV();

        // Update name
        name = this.ToString();
    }

    private void OnBlur(ResourceToken token)
    {
        if (this.token != token) return;

        // TODO
    }

    private void OnDrop(TokenStack stack, bool state)
    {
        // if (this.token != token) return;

        // TODO
    }

    private void OnMouseEnter()
    {
        // Select token
        token.SelectToken(true);
    }

    private void OnMouseExit()
    {
        // Deselect token
        token.SelectToken(false);
    }

    private void OnMouseDown()
    {
        // This is the controller -> logic

        // ?
        TransferHandler.instance.GatherTransport(); // ?

        // Drag tokens
        token.DragTokens();
    }

    private void OnMouseDrag()
    {
        // Do nothing
    }

    private void OnMouseUp()
    {
        // Stop selecting
        token.SelectToken(false);

        // Drop transport
        TransferHandler.instance.DropTransport();
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


    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    private void OnHover(ResourceToken token, bool state)
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

    private void OnDrag(ResourceToken token)
    {
        // This acts as the logic -> visual

        // Make sure it's this token
        if (this.token != token) return;
        
        // Add to transport
        TransferHandler.instance.PickupTransport(this);
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

    private void MoveToken()
    {
        if (this.stackHandler != null)
        {
            // Debug
            print(stackHandler.ToString());

            // Move
            MoveToStack(this.stackHandler);
        }
    }

    private void DestroyToken(ResourceToken token)
    {
        if (this.token != token) return;

        // Destroy self
        Destroy(gameObject);
    }

    private void Relocate(StackHandler stackHandler)
    {
        // Change parent
        transform.parent = stackHandler.GetTokenTransform();

        // Set new home
        homePosition = stackHandler.GetTokenPosition();

        // Move to home
        transform.position = homePosition;
    }

    public void MoveToStack(StackHandler stackHandler)
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

            // Relocation token
            Relocate(stackHandler);
        }
    }

    public void ReturnToStack()
    {
        if (stackHandler != null) MoveToStack(stackHandler);
        else throw new System.Exception("ERROR!");
    }

    private void EnableOutline()
    {
        outline.eraseRenderer = false;
    }

    private void DisableOutline()
    {
        outline.eraseRenderer = true;
    }

    public void ToggleGravity(bool state)
    {
        body.useGravity = state;
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

        debugText.text = token.ToString();
    }
}
