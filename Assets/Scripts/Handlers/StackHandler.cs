using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject tokenPrefab;
    [SerializeField] private Transform tokenDropPosition;

    [Header("Data")]
    [SerializeField] private TokenStack tokenStack;

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

    public void Initialize(TokenStack tokenStack)
    {
        this.tokenStack = tokenStack;

        // TODO?
    }

    private void CreateToken(ResourceToken token, TokenStack tokenStack)
    {
        // If this stack wasnt choosen
        if (this.tokenStack != tokenStack) return;

        // Create the token object
        var tokenHandler = Instantiate(tokenPrefab).GetComponent<TokenHandler>();
        // Initialize it
        tokenHandler.Initialize(token, tokenDropPosition);
    }

    public Transform GetTokenTransform()
    {
        return tokenDropPosition;
    }

    public TokenStack GetTokenStack()
    {
        return tokenStack;
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tokenDropPosition.position, debugRadius);
    }
}
