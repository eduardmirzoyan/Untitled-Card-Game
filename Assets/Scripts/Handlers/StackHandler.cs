using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject tokenPrefab;
    [SerializeField] private Transform tokenDropPosition;
    [SerializeField] private MeshRenderer meshRenderer;

    [Header("Data")]
    [SerializeField] private TokenStack stack;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private float debugRadius = 0.1f;

    private void Start()
    {
        // Sub
        TokenEvents.instance.onCreate += CreateToken;
        TokenEvents.instance.onDrop += ToggleHighlight;
    }

    private void OnDestroy()
    {
        // Unsub
        TokenEvents.instance.onCreate -= CreateToken;
        TokenEvents.instance.onDrop -= ToggleHighlight;
    }

    public void Initialize(TokenStack stack)
    {
        this.stack = stack;

        // TODO?
    }

    public Vector3 GetTokenPosition()
    {
        if (stack == null) return tokenDropPosition.position;

        float tokenThickness = 0.1f;

        // Calculate position to add token based on stack
        return tokenDropPosition.position + Vector3.up * tokenThickness * stack.GetSize();
    }

    private void CreateToken(ResourceToken token, TokenStack stack)
    {
        // If this stack wasnt choosen
        if (this.stack != stack) return;

        // Create the token object
        var tokenHandler = Instantiate(tokenPrefab).GetComponent<TokenHandler>();
        // Initialize it
        tokenHandler.Initialize(token, this);
    }

    private void ToggleHighlight(TokenStack stack, bool state)
    {
        // Make sure it's this slot
        if (this.stack != stack) return;

        if (state)
        {
            // Highlight this slot
            EnableHighlight();
        }
        else
        {
            // Remove highlight
            DisableHighlight();
        }
    }

    private void EnableHighlight()
    {
        // Show mesh
        meshRenderer.enabled = true;
    }

    private void DisableHighlight()
    {
        // Hide mesh
        meshRenderer.enabled = false;
    }

    public Transform GetTokenTransform()
    {
        return tokenDropPosition;
    }

    public TokenStack GetTokenStack()
    {
        return stack;
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetTokenPosition(), debugRadius);
    }
}
