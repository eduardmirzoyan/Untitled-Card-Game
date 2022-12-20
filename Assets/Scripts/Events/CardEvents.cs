using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardEvents : MonoBehaviour
{
    public static CardEvents instance;
    private void Awake()
    {
        // Singleton Logic
        if (CardEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<Card> onCreate;
    public event Action<Card> onMove;
    public event Action<Card> onDestroy;

    public void TriggerOnCreate(Card card)
    {
        if (onCreate != null)
        {
            onCreate(card);
        }
    }

    public void TriggerOnMove(Card card)
    {
        if (onMove != null)
        {
            onMove(card);
        }
    }

    public void TriggerOnDestroy(Card card)
    {
        if (onDestroy != null)
        {
            onDestroy(card);
        }
    }
}
