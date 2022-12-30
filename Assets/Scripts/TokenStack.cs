using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TokenStack : ScriptableObject
{
    public int stackLimit = 5;
    public List<ResourceToken> tokens;
    public List<ResourceToken> selectedTokens;

    public void Initialize(int stackLimit = 5)
    {
        this.stackLimit = stackLimit;

        // Initialize stack
        tokens = new List<ResourceToken>();

        // Initialize list
        selectedTokens = new List<ResourceToken>();
    }

    public void PushToken(ResourceToken token)
    {
        // Add token to end of stack
        tokens.Add(token);
    }

    public ResourceToken PopToken()
    {
        // Return top token, retun null if empty
        if (tokens.Count == 0) return null;

        // Save removed token
        var removedToken = tokens[tokens.Count - 1];

        // Remove top token
        tokens.RemoveAt(tokens.Count - 1);

        // Return removed token
        return removedToken;
    }

    public bool IsFull()
    {
        return tokens.Count >= stackLimit;
    }

    public void ClearStack()
    {
        // Remove all tokens
        while (tokens.Count > 0)
        {
            // Remove the token from the stack
            var token = PopToken();

            // Destroy the removed token
            token.Destroy();
        }
    }

    public void SelectToken(ResourceToken selectedToken, bool state)
    {
        bool tokenFound = false;
        // Check if you are selecting
        if (state)
        {
            // Check if token exists
            foreach (var token in tokens)
            {
                // If token was already found or is now found
                if (tokenFound || token == selectedToken)
                {
                    // Add to selected
                    selectedTokens.Add(token);

                    // Trigger event
                    TokenEvents.instance.TriggerOnSelect(token, true);

                    // Trigger flag
                    tokenFound = true;
                }
            }
        }
        // Or deselecting
        else
        {
            // Clear all selected tokens 
            foreach (var token in selectedTokens)
            {
                // Trigger event
                TokenEvents.instance.TriggerOnSelect(token, false);
            }

            // Clear tokens list
            selectedTokens.Clear();
        }
        
    }
}
