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
    [SerializeField] private Collider hitbox;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Data")]
    [SerializeField] private ResourceToken token;
    [SerializeField] private Sprite[] tokenSprites;
    [SerializeField] private Color[] tokenColors;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private bool randomColor;

    private StackHandler stackHandler;

    private void Start()
    {
        // Sub
        TokenEvents.instance.onHover += OnHover;
        TokenEvents.instance.onBlur += OnBlur;
        TokenEvents.instance.onDestroy += DestroyToken;
    }

    private void OnDestroy()
    {
        // Unsub
        TokenEvents.instance.onHover -= OnHover;
        TokenEvents.instance.onBlur -= OnBlur;
        TokenEvents.instance.onDestroy -= DestroyToken;
    }

    public void Initialize(ResourceToken token, StackHandler stackHandler)
    {
        this.token = token;
        this.stackHandler = stackHandler;

        // Set Icon based on type
        spriteRenderer.sprite = tokenSprites[(int)token.tokenType];

        // Set Color based on type
        if (randomColor)
            meshRenderer.material.color = UnityEngine.Random.ColorHSV();
        else
            meshRenderer.material.color = tokenColors[(int) token.tokenType];

        // Disable outline
        DisableOutline();

        // Set home
        Relocate(stackHandler);

        // Update name
        name = this.ToString();
    }

    private void OnMouseEnter()
    {
        // Select token
        token.SelectToken();
    }

    private void OnMouseExit()
    {
        // Deselect token
        token.DeselectToken();
    }

    private void OnMouseDown()
    {
        // Pick up all selected tokens
        TransferHandler.instance.PickupTokens();
    }

    private void OnMouseDrag()
    {
        // Do nothing
    }

    private void OnMouseUp()
    {
        // Drop all selected tokens
        TransferHandler.instance.DropTokens();
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

    private void OnHover(ResourceToken token)
    {
        // Make sure it's this token
        if (this.token != token) return;

        // Highlight this token
        EnableOutline();

        // Select this token
        TransferHandler.instance.SelectToken(this);
    }

    private void OnBlur(ResourceToken token)
    {
        // Make sure it's this token
        if (this.token != token) return;

        // Remove highlight
        DisableOutline();

        // Deselect this token
        TransferHandler.instance.DeselectToken(this);
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

    public void EnablePhysics(bool state)
    {
        body.useGravity = state;
        hitbox.enabled = state;
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

        if (token != null)
            debugText.text = token.ToString();
    }
}
