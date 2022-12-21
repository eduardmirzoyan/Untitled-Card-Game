using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Board : ScriptableObject
{
    public int width;
    public int height;

    public CardSlot[,] cardSlots;

    public void Initialize(int width, int height)
    {
        this.width = width;
        this.height = height;
        cardSlots = new CardSlot[width, height];

        // Create card slot array
        for (int i = 0; i < cardSlots.GetLength(0); i++)
        {
            for (int j = 0; j < cardSlots.GetLength(1); j++)
            {
                // Create slot
                var cardSlot = ScriptableObject.CreateInstance<CardSlot>();
                cardSlot.Initialize(new Vector2Int(i, j), this);

                // Assign it
                cardSlots[i, j] = cardSlot;

                // Debug
                Debug.Log("Creating slot: " + cardSlot.ToString());
            }
        }

        // Debug
        Debug.Log("Created new board w/ size: " + cardSlots.GetLength(0) + " x " + cardSlots.GetLength(1));

        // Trigger event
        BoardEvents.instance.TriggerOnInitalize(this);
    }

    public void CreateCard(Card card)
    {
        // Find the first open position
        foreach (var cardslot in cardSlots)
        {
            // Check to see if slot is empty
            if (!cardslot.IsOccupied())
            {
                // Create the card here
                CreateCard(card, cardslot.position);
                
                // Finish
                break;
            }
        }

        // Else the board is full and a card cannot be made
        Debug.Log("Board is full, card cannot be created.");
    }

    public void CreateCard(Card card, Vector2Int position)
    {
        // Get card slot
        var cardSlot = cardSlots[position.x, position.y];

        // Make sure transfer is possible
        if (cardSlot.IsOccupied())
        {
            // Debug
            Debug.Log("Position " + position + " already has a card.");

            // Finish
            return;
        }

        // Debug
        Debug.Log("Creating card at " + cardSlot.ToString());

        // Give the card to the cardslot
        cardSlot.SetCard(card);

        // Initalize the card
        card.Initialize(cardSlot);

        // Trigger event
        BoardEvents.instance.TriggerOnCreateCard(card, position);
    }

    public void CreateToken(ResourceToken token)
    {
        // Find the first open position
        foreach (var cardslot in cardSlots)
        {
            // Check to see if slot is has a card
            if (cardslot.IsOccupied())
            {
                // Create a token on the card
                CreateToken(token, cardslot.card);

                // Finish
                break;
            }
        }

        // Else the board does not have cards to create tokens on
        Debug.Log("Board is does not have any cards that can hold tokens.");
    }

    public void CreateToken(ResourceToken token, TokenStack stack)
    {
        // Add to card's input stack
        // card.AddTokenToInput(token);

        // Trigger event
        // TokenEvents.instance.TriggerOnCreate(token, card);
    }

    public void CreateToken(ResourceToken token, Card card)
    {
        // Add to card's input stack
        card.AddTokenToInput(token);

        // Trigger event
        TokenEvents.instance.TriggerOnCreate(token, card);
    }

    public void MoveCard(Card card, Vector2Int oldPosition, Vector2Int newPosition)
    {
        // Get respective slots
        var oldSlot = cardSlots[oldPosition.x, oldPosition.y];
        var newSlot = cardSlots[newPosition.x, newPosition.y];

        // Make sure transfer is possible
        if (newSlot.IsOccupied())
        {
            // Debug
            Debug.Log("Position " + newPosition + " already has a card.");

            // Finish
            return;
        }

        // Set old slot to empty
        oldSlot.SetCard(null);

        // Set new slot to the card
        newSlot.SetCard(card);

        // Trigger event
        BoardEvents.instance.TriggerOnMoveCard(card, oldPosition, newPosition);
    }

    public string ToStringVerbose()
    {
        string output = "";

        for (int i = 0; i < cardSlots.GetLength(0); i++)
        {
            for (int j = 0; j < cardSlots.GetLength(1); j++)
            {
                output += cardSlots[i, j].ToString();
            }
            output += "\n";
        }

        return output;
    }

    public string ToStringSimple()
    {
        string output = "";

        for (int i = 0; i < cardSlots.GetLength(1); i++)
        {
            for (int j = 0; j < cardSlots.GetLength(0); j++)
            {
                if (cardSlots[j, i].IsOccupied())
                    output += "[x]";
                else
                    output += "[  ]";
            }
            output += "\n";
        }

        return output;
    }
}
