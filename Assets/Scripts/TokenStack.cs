using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TokenStack : ScriptableObject
{
    public int stackLimit = 5;
    public Stack<ResourceToken> tokens;
    public int size;

    public void Initialize(int stackLimit = 5)
    {
        this.stackLimit = stackLimit;

        // Initialize stack
        tokens = new Stack<ResourceToken>();
        size = 0;
    }

    public void PushToken(ResourceToken token)
    {
        // Add token to stack
        tokens.Push(token);
        size++;

        // Trigger event?
    }

    public ResourceToken PopToken()
    {
        // Return top token, retun null if empty
        if (tokens.Count == 0) return null;

        // Trigger event?

        size--;
        return tokens.Pop();
    }

    public bool IsFull()
    {
        return size >= stackLimit;
    }

    public void ClearStack()
    {
        // Remove all tokens
        while (tokens.Count > 0)
        {
            // Remove the token from the stack
            var token = tokens.Pop();

            // Destroy the token
            token.Destroy();
        }
        
        size = 0;
    }

    public void SelectToken(ResourceToken token)
    {
        bool isSelected = false;
        // Loop through tokens
        for (int i = 0; i < size; i++)
        {
            // CHANGE TO ARRAY!!!

            // If this token was found at i
            if (isSelected) 
            {
                // Trigger event
                // instance.TriggerOnStartHover(token, stack)
            }
            else if (tokens.Peek() == token)
            {
                // Highlight this token and all above it
                // instance.TriggerOnStartHover(token, stack)

                isSelected = true;
            }
        }
    }
}
