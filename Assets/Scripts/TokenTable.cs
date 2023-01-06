using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TokenTable : ScriptableObject
{
    [SerializeField] private ResourceToken[] tokens;

    public void Initialize()
    {
        var tokenTypes = System.Enum.GetValues(typeof(TokenType));

        // Init array
        tokens = new ResourceToken[tokenTypes.Length];
        // Loop through each token type
        foreach (TokenType tokenType in tokenTypes)
        {
            int typeIndex = (int) tokenType;
            // Create token
            tokens[typeIndex] = ScriptableObject.CreateInstance<ResourceToken>();
            tokens[typeIndex].tokenType = tokenType;

            // Set name
            tokens[typeIndex].name = tokens[typeIndex].BasicString();
        }
    }

    public ResourceToken GetRandomToken()
    {
        return tokens[Random.Range(0, tokens.Length)];
    }
}
