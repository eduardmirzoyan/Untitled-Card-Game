using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenType { Food, Faith, Gold, Manpower };

[CreateAssetMenu]
public class ResourceToken : ScriptableObject
{
    public new string name;
    public TokenType tokenType;
    public TokenStack stack;
    public Sprite sprite;
    public Color color;

    public void Initialize(TokenType tokenType, TokenStack stack)
    {
        this.tokenType = tokenType;
        this.stack = stack;
    }

    public void SelectToken()
    {
        // Get list of tokens starting from this
        var selectedTokens = stack.GetTokensUntilTop(this);
        foreach (var token in selectedTokens)
        {
            // Trigger event
            TokenEvents.instance.TriggerOnHover(token);
        }
    }

    public void DeselectToken()
    {
        // Deselect all tokens in stack
        foreach (var token in stack.tokens)
        {
            // Trigger event
            TokenEvents.instance.TriggerOnBlur(token);
        }
    }

    public bool MoveTo(TokenStack newStack)
    {
        var oldStack = this.stack;

        // Check if previous stack exists
        if (oldStack != null)
        {
            // Remove old token
            oldStack.PopToken();
        }

        // Add new token
        newStack.PushToken(this);

        // Update card slot
        this.stack = newStack;

        // Debug
        Debug.Log(this.ToString() + " was moved.");
        
        // Return result
        return true;
    }

    public void Destroy()
    {
        this.stack = null;

        // Trigger event
        TokenEvents.instance.TriggerOnDestroy(this);
    }

    public override string ToString()
    {
        return tokenType.ToString() + " [" + this.GetInstanceID() + "]";
    }
}
