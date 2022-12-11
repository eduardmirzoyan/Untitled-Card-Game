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

    public event Action<Card> onMove;

    public void TriggerOnMove(Card card)
    {
        if (onMove != null)
        {
            onMove(card);
        }
    }
}
