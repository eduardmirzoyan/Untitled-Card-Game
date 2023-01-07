using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Event, Character, Building, Item, Objective }

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [Header("Basics")]
    public new string name;
    public string flavorText;
    public CardType cardType;
    public float lifetime = 30f;
    public float usetime;
    public int numUses = 1; // -1 => inf

    [Header("Effect Details")]
    public string effectDescription;
    public Cost cost;
    public Reward reward;

    [Header("Visuals?")]
    public Sprite sprite;

    [Header("Token Stacks")]
    public TokenStack inputStack;
    public TokenStack outputStack;

    [Header("Parent")]
    public CardSlot cardSlot;

    [Header("Runtime")]
    public float lifeCounter;
    public float useCounter;
    public bool setToDestroy;

    public void Create(CardSlot cardSlot)
    {
        this.cardSlot = cardSlot;
        cardSlot.card = this;

        // Initialize stacks
        inputStack = ScriptableObject.CreateInstance<TokenStack>();
        inputStack.Initialize();

        outputStack = ScriptableObject.CreateInstance<TokenStack>();
        outputStack.Initialize();

        // Initialize timers
        lifeCounter = lifetime;
        useCounter = usetime;
        setToDestroy = false;

        // Debug
        Debug.Log("Created: " + this.ToString());

        // Trigger event
        CardEvents.instance.TriggerOnCreate(this, cardSlot);
    }

    public bool MoveTo(CardSlot newSlot)
    {
        // Make sure slot is free
        if (newSlot.IsOccupied()) return false;

        var oldSlot = this.cardSlot;

        // Check if previous slot exists
        if (oldSlot != null)
        {
            // Make empty
            oldSlot.card = null;
        }

        // Set new slot
        newSlot.card = this;

        // Update card slot
        this.cardSlot = newSlot;

        // Trigger event
        CardEvents.instance.TriggerOnMove(this, oldSlot, newSlot);

        return true;
    }

    public void SetReadyToDestroy()
    {
        // Set flag
        setToDestroy = true;

        // Trigger event
        CardEvents.instance.TriggerOnSetToDestroy(this);
    }

    public void AttemptToDestroy()
    {
        // See if card can be destroyed
        if (setToDestroy && inputStack.IsEmpty() && outputStack.IsEmpty())
        {
            // Debug
            Debug.Log("Destroyed: " + this.ToString());

            // Remove from stack
            cardSlot.card = null;

            // Set cardslot to null
            cardSlot = null;

            // Trigger event
            CardEvents.instance.TriggerOnDestroy(this);
        }
    }

    public void TickTimers()
    {
        // Don't do anything if card should be destroyed
        if (setToDestroy) return;

        // Check if cost is satified
        if (CostSatified())
        {
            // If there is use time, then tick
            if (useCounter > 0)
            {
                // Decrement time
                useCounter -= Time.deltaTime;

                // Trigger event
                CardEvents.instance.TriggerOnTickUse(this);

                // If timer reaches 0
                if (useCounter <= 0)
                {
                    // Activate effect
                    Trigger();
                }
            }
        }
        // Else tick down lifetime
        else if (lifeCounter > 0)
        {
            // Decrement time
            lifeCounter -= Time.deltaTime;

            // Trigger event
            CardEvents.instance.TriggerOnTickLife(this);

            // If timer reaches 0
            if (lifeCounter <= 0)
            {
                // Destroy this object
                SetReadyToDestroy();
                
                // Destroy();

            }
        }
    }

    public void ResetLifetime()
    {
        // Reset counter
        lifeCounter = lifetime;

        // Trigger event
        CardEvents.instance.TriggerOnTickLife(this);
    }

    public void ResetUsetime()
    {
        // Reset counter
        useCounter = usetime;

        // Trigger event
        CardEvents.instance.TriggerOnTickUse(this);
    }

    public bool CostSatified()
    {
        // Check if the card's costs have been satisfied
        return cardType == CardType.Event || cost.IsSatisfied(this, inputStack);
    }

    public virtual void Trigger()
    {
        // Destroy all tokens in the input stack
        inputStack.DestroyStack();

        // Yield card rewards
        reward.Generate(this, outputStack);

        // Reset use timers
        ResetUsetime();

        // Reduce num uses
        DecrementUses();

        // Debug
        Debug.Log(this.ToString() + " Triggered!");

        // Trigger event
        CardEvents.instance.TriggerOnTrigger(this);
    }

    private void DecrementUses()
    {
        // Check if you have any uses left
        if (numUses > 0)
        {
            // Decrement
            numUses--;

            if (numUses == 0)
            {
                // Destroy this card
                SetReadyToDestroy();

                // Destroy();
            }
        }
    }

    public string GetUses()
    {
        // Return infinity char or value
        return numUses == -1 ? "\u221E" : "" + numUses;
    }

    public string GetLifetime()
    {
        // Return infinity char or value
        return lifetime == -1 ? "\u221E" : "" + lifeCounter;
    }

    public string GetUsetime()
    {
        // Return infinity char or value
        return usetime == -1 ? "\u221E" : "" + usetime;
    }

    public override string ToString()
    {
        return "'" + name + "'" + " [" + cardType.ToString() + " Card]";
    }
}
