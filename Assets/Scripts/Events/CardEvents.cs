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
    public event Action<Card, bool> onDrag;
    public event Action<CardSlot, bool> onDrop;
    public event Action<Card, CardSlot, CardSlot> onMove;
    public event Action<Card> onDestroy;

    public event Action<Card> onTickLife;
    public event Action<Card> onTickUse;
    public event Action<Card> onUseEffect;

    public void TriggerOnCreate(Card card)
    {
        if (onCreate != null)
        {
            onCreate(card);
        }
    }

    public void TriggerOnDrag(Card card, bool state)
    {
        if (onDrag != null)
        {
            onDrag(card, state);
        }
    }

    public void TriggerOnDrop(CardSlot cardSlot, bool state)
    {
        if (onDrop != null)
        {
            onDrop(cardSlot, state);
        }
    }

    public void TriggerOnMove(Card card, CardSlot fromSlot, CardSlot toSlot)
    {
        if (onMove != null)
        {
            onMove(card, fromSlot, toSlot);
        }
    }

    public void TriggerOnDestroy(Card card)
    {
        if (onDestroy != null)
        {
            onDestroy(card);
        }
    }

    public void TriggerOnTickLife(Card card)
    {
        if (onTickLife != null)
        {
            onTickLife(card);
        }
    }

    public void TriggerOnTickUse(Card card)
    {
        if (onTickUse != null)
        {
            onTickUse(card);
        }
    }

    public void TriggerOnUseEffect(Card card)
    {
        if (onUseEffect != null)
        {
            onUseEffect(card);
        }
    }
}
