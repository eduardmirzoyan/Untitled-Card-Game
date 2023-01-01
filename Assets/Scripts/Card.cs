using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Character, Building, Item, Objective }

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [Header("Basics")]
    public new string name;
    public CardType cardType;
    public float lifetime;
    public float usetime;
    public int numUses; // -1 => inf

    [Header("Effect Details")]
    public string effectDescription;
    public Cost cost;

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

    public void Initialize(CardSlot cardSlot = null)
    {
        this.cardSlot = cardSlot;

        // Initalize stacks
        inputStack = ScriptableObject.CreateInstance<TokenStack>();
        inputStack.Initialize();

        outputStack = ScriptableObject.CreateInstance<TokenStack>();
        outputStack.Initialize();

        // Intialize timers
        lifeCounter = lifetime;
        useCounter = usetime;
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
            oldSlot.SetCard(null);
        }

        // Set new slot
        newSlot.SetCard(this);

        // Update card slot
        this.cardSlot = newSlot;

        // Trigger event
        CardEvents.instance.TriggerOnMove(this, oldSlot, newSlot);

        return true;
    }

    public bool SatisfiesCost()
    {
        // TODO
        return false;
    }

    public void TickTimers()
    {
        // Check for effect timer first
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
                ActivateEffect();
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
                Destroy();
            }
        }
    }

    public bool CheckRequirements()
    {
        // TODO
        return true;
    }

    public virtual void ActivateEffect()
    {
        // Destroy all input tokens
        inputStack.DestroyStack();

        // Perform the actual effect here
        // TODO

        // Create output
        // TODO

        // Reset timer
        // TODO

        // Trigger event
        CardEvents.instance.TriggerOnUseEffect(this);

        // Check if you have any uses left
        if (numUses > 0)
        {
            // Decrement
            numUses--;

            if (numUses == 0)
            {
                // Destroy this
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        // TODO

        // Trigger event
        CardEvents.instance.TriggerOnDestroy(this);
    }

    public override string ToString()
    {
        return name + " [" + cardType.ToString() + "] Card";
    }
}
