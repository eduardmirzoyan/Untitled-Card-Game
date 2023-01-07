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

    public event Action<Card, CardSlot> onCreate;
    public event Action<CardSlot, bool> onDrop;
    public event Action<Card, CardSlot, CardSlot> onMove;
    public event Action<Card> onSetToDestroy;
    public event Action<Card> onDestroy;

    public event Action<Card> onTickLife;
    public event Action<Card> onTickUse;
    public event Action<Card> onTrigger;

    public event Action<Card> onHover;
    public event Action<Card> onBlur;

    public event Action<Card> onInspect;
    public event Action<Card> onUninspect;

    public void TriggerOnCreate(Card card, CardSlot cardSlot)
    {
        if (onCreate != null)
        {
            onCreate(card, cardSlot);
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

    public void TriggerOnSetToDestroy(Card card)
    {
        if (onSetToDestroy != null)
        {
            onSetToDestroy(card);
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

    public void TriggerOnTrigger(Card card)
    {
        if (onTrigger != null)
        {
            onTrigger(card);
        }
    }

    public void TriggerOnInspect(Card card)
    {
        if (onInspect != null)
        {
            onInspect(card);
        }
    }

    public void TriggerOnUninspect(Card card)
    {
        if (onUninspect != null)
        {
            onUninspect(card);
        }
    }

    public void TriggerOnHover(Card card)
    {
        if (onHover != null)
        {
            onHover(card);
        }
    }

    public void TriggerOnBlur(Card card)
    {
        if (onBlur != null)
        {
            onBlur(card);
        }
    }

}
