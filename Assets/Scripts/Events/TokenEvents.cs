using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TokenEvents : MonoBehaviour
{
    public static TokenEvents instance;
    private void Awake()
    {
        // Singleton Logic
        if (TokenEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<ResourceToken, TokenStack> onCreate;
    public event Action<ResourceToken, TokenStack, TokenStack> onMove;
    public event Action<ResourceToken> onDestroy;

    public event Action<TokenHandler, StackHandler> onMoveWorld;

    public void TriggerOnCreate(ResourceToken token, TokenStack stack)
    {
        if (onCreate != null)
        {
            onCreate(token, stack);
        }
    }

    public void TriggerOnMove(ResourceToken token, TokenStack fromStack, TokenStack toStack)
    {
        if (onMove != null)
        {
            onMove(token, fromStack, toStack);
        }
    }

    public void TriggerOnDestroy(ResourceToken token)
    {
        if (onDestroy != null)
        {
            onDestroy(token);
        }
    }
}
