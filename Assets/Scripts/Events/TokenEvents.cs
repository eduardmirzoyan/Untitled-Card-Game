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

    public event Action<ResourceToken, Card> onCreate;
    public event Action<ResourceToken, TokenStack, TokenStack> onMove;
    public event Action<ResourceToken> onDestroy;

    public void TriggerOnCreate(ResourceToken token, Card card)
    {
        if (onCreate != null)
        {
            onCreate(token, card);
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
