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
    public float useTime;
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

    public void Initialize(CardSlot cardSlot = null)
    {
        this.cardSlot = cardSlot;
        
        // Initalize stacks
        inputStack = ScriptableObject.CreateInstance<TokenStack>();
        inputStack.Initialize();

        outputStack = ScriptableObject.CreateInstance<TokenStack>();
        outputStack.Initialize();
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

    public void AddTokenToInput(ResourceToken token)
    {
        // Add token to stack
        inputStack.PushToken(token);
    }

    public bool SatisfiesCost()
    {
        // TODO
        return false;
    }

    public virtual void ActivateEffect()
    {
        // Clear input tokens
        inputStack.ClearStack();

        // Perform the actual effect here
        // TODO

        // Create output

        // Check if you have any uses left
        if (numUses > 0)
        {
            // Decrement
            numUses--;

            if (numUses == 0)
            {
                // Destroy this
            }
        }
    }
}
