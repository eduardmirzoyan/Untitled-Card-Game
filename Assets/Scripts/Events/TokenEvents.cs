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

    public event Action<ResourceToken> onHover;
    public event Action<ResourceToken> onBlur;

    public event Action<ResourceToken> onDestroy;

    public void TriggerOnCreate(ResourceToken token, TokenStack stack)
    {
        if (onCreate != null)
        {
            onCreate(token, stack);
        }
    }

    public void TriggerOnHover(ResourceToken token)
    {
        if (onHover != null)
        {
            onHover(token);
        }
    }

    public void TriggerOnBlur(ResourceToken token)
    {
        if (onBlur != null)
        {
            onBlur(token);
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
