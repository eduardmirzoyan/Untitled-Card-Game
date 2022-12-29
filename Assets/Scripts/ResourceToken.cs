using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenType { Food, Faith, Gold, Manpower };

[CreateAssetMenu]
public class ResourceToken : ScriptableObject
{
    public TokenType tokenType;
    public TokenStack stack;

    public void Initialize(TokenType tokenType, TokenStack stack)
    {
        this.tokenType = tokenType;
        this.stack = stack;
    }

    public bool MoveTo(TokenStack newStack)
    {
        var oldStack = this.stack;

        // Debug
        Debug.Log(ToString() + " was moved to " + newStack.name);

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

        // Trigger event
        TokenEvents.instance.TriggerOnMove(this, oldStack, newStack);
        
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
        return tokenType.ToString() + " Token";
    }
}
