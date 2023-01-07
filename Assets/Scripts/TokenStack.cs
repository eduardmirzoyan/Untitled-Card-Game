using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TokenStack : ScriptableObject
{
    public int stackLimit = 5;
    public List<ResourceToken> tokens;
    public SortedSet<TokenType> allowedTypes;
    public bool isLocked;

    public void Initialize(int stackLimit = 5)
    {
        this.stackLimit = stackLimit;

        // Initialize lists
        tokens = new List<ResourceToken>();
        allowedTypes = new SortedSet<TokenType>();
    }

    public void AddWhiteList(TokenType tokenType)
    {
        allowedTypes.Add(tokenType);
    }

    public bool CanAddToken(ResourceToken token)
    {
        return !isLocked && !IsFull() && allowedTypes.Contains(token.tokenType);
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

    public bool IsEmpty()
    {
        return tokens.Count == 0;
    }

    public bool IsFull()
    {
        return tokens.Count >= stackLimit;
    }

    public void DestroyStack()
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

    public int GetSize()
    {
        return tokens.Count;
    }

    public override string ToString()
    {
        return "Token Stack";
    }
}
