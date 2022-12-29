using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Board : ScriptableObject
{
    public int width;
    public int height;

    public CardSlot[,] cardSlots;

    public TokenStack[] resourceStacks;
    public int numStacks;
    public int stackCap = 10;

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

        // Create side board
        InitializeSide();

        // Trigger event
        BoardEvents.instance.TriggerOnInitalize(this);
    }

    private void InitializeSide()
    {
        int numStacks = System.Enum.GetValues(typeof(TokenType)).Length;

        // Create new list
        resourceStacks = new TokenStack[numStacks];

        // Initialize stacks
        for (int i = 0; i < resourceStacks.Length; i++)
        {
            // Create SO
            resourceStacks[i] = ScriptableObject.CreateInstance<TokenStack>();
            // Initialize
            resourceStacks[i].Initialize(stackCap);
        }

        this.numStacks = numStacks;
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
        // Loop through stockpile
        foreach (var stack in resourceStacks)
        {
            if (!stack.IsFull())
            {
                // Create token on that stack
                CreateToken(token, stack);
                // Stop
                break;
            }
        }
    }

    public void CreateToken(ResourceToken token, TokenStack stack)
    {
        // Debug
        Debug.Log("Creating token: " + token.name);

        // Add token to stack
        stack.PushToken(token);

        // Trigger event
        TokenEvents.instance.TriggerOnCreate(token, stack);

        // Add to card's input stack
        // card.AddTokenToInput(token);

        // Trigger event
        // TokenEvents.instance.TriggerOnCreate(token, card);
    }

    public void CreateToken(ResourceToken token, Card card)
    {
        // TODO remove this

        // Add to card's input stack
        card.AddTokenToInput(token);

        // Trigger event
        TokenEvents.instance.TriggerOnCreate(token, null);
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
