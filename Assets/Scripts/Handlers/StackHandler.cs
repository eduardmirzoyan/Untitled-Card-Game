using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TokenStack tokenStack;

    private void Initialize(TokenStack tokenStack)
    {
        this.tokenStack = tokenStack;

        // TODO
    }

    public TokenStack GetTokenStack()
    {
        return tokenStack;
    }
}
