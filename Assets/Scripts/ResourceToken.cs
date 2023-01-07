using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenType { Fealty, Population, Food, Faith, Gold, Manpower };

[CreateAssetMenu]
public class ResourceToken : ScriptableObject
{
    [Header("Basic")]
    public TokenType tokenType;

    [Header("Parent")]
    public TokenStack stack;

    public void Create(TokenStack stack)
    {
        this.stack = stack;

        // Add this token to stack
        stack.PushToken(this);

        // Debug
        Debug.Log("Created: " + this.ToString());

        // Trigger event
        TokenEvents.instance.TriggerOnCreate(this, stack);
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

        // Debug
        Debug.Log("Destroyed: " + this.ToString());

        // Trigger event
        TokenEvents.instance.TriggerOnDestroy(this);
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

    public override string ToString()
    {
        return tokenType.ToString() + " Token " + " [" + this.GetInstanceID() + "]";
    }

    public string BasicString()
    {
        return tokenType.ToString() + " Token";
    } 
}
