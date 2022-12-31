using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public List<ResourceToken> GetTokensUntilTop(ResourceToken token)
    {
        // Get index of token
        int index = tokens.IndexOf(token);

        // Error handling
        if (index < 0) throw new System.Exception("TOKEN NOT FOUND: " + token.ToString());

        // Loop through all tokens
        List<ResourceToken> result = new List<ResourceToken>();
        for (int i = 0; i < tokens.Count; i++)
        {
            // If they are at or greater than the index, then add em
            if (i >= index)
            {
                result.Add(tokens[i]);
            }
        }

        return result;
    }

    public void SelectToken(ResourceToken selectedToken, bool state)
    {
        // Check if you are selecting
        if (state)
        {
            // Init flag
            bool tokenFound = false;

            // Check if token exists
            foreach (var token in tokens)
            {
                // If token was already found or is now found
                if (tokenFound || token == selectedToken)
                {
                    // Add to selected
                    selectedTokens.Add(token);

                    // Trigger event
                    TokenEvents.instance.TriggerOnHover(token, true);

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
                TokenEvents.instance.TriggerOnHover(token, false);
            }

            // Clear tokens list
            selectedTokens.Clear();
        }   
    }

    public void DragSelectedTokens()
    {
        // Add each selected token for transport
        foreach (var token in selectedTokens)
        {
            // Trigger event
            TokenEvents.instance.TriggerOnDrag(token);
        }
    }

    public int GetSize()
    {
        return tokens.Count;
    }
}
