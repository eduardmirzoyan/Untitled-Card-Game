using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject tokenPrefab;
    [SerializeField] private Transform tokenDropPosition;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer outlineMesh;

    [Header("Data")]
    [SerializeField] private TokenStack stack;

    [Header("Settings")]
    [SerializeField] private float tokenSpacing = 0.2f;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private float debugRadius = 0.1f;

    private void Start()
    {
        // Sub
        TokenEvents.instance.onCreate += CreateToken;
    }

    private void OnDestroy()
    {
        // Unsub
        TokenEvents.instance.onCreate -= CreateToken;
    }

    public void Initialize(TokenStack stack)
    {
        this.stack = stack;

        // Change name
        name = this.ToString();
    }

    public void Initialize(TokenStack stack, Color color)
    {
        // Init
        Initialize(stack);

        // Set outline color
        outlineMesh.material.color = color;
    }

    public Vector3 GetTokenPosition()
    {
        if (stack == null) return tokenDropPosition.position;

        // Calculate position to add token based on stack
        return tokenDropPosition.position + Vector3.up * tokenSpacing * stack.GetSize();
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

    public void EnableHighlight()
    {
        // Show mesh
        meshRenderer.enabled = true;
    }

    public void DisableHighlight()
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

    public override string ToString()
    {
        return stack.ToString() + " Object";
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetTokenPosition(), debugRadius);
    }
}
