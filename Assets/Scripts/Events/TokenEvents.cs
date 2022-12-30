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
    public event Action<ResourceToken, bool> onSelect;
    public event Action<ResourceToken, TokenStack, TokenStack> onMove;
    public event Action<ResourceToken> onDestroy;

    // ?
    public event Action<ResourceToken, bool> onMoveSelect;

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

    public void TriggerOnSelect(ResourceToken token, bool state)
    {
        if (onSelect != null)
        {
            onSelect(token, state);
        }
    }

    // TODO
    public void TriggerOnMoveSelect(ResourceToken token, bool state)
    {
        if (onMoveSelect != null)
        {
            onMoveSelect(token, state);
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
