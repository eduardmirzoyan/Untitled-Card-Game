using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TokenStack : ScriptableObject
{
    public int stackLimit = 5;
    public Stack<ResourceToken> tokens;
    public int size;

    public void Initialize()
    {
        // Initialize stack
        tokens = new Stack<ResourceToken>();
        size = 0;
    }

    public void PushToken(ResourceToken token)
    {
        // Add token to stack
        tokens.Push(token);
        size++;
    }

    public ResourceToken PopToken()
    {
        // Return top token, retun null if empty
        if (tokens.Count == 0) return null;

        size--;
        return tokens.Pop();
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
}
