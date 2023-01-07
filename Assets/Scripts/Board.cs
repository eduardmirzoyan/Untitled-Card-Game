using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Board : ScriptableObject
{
    public int width;
    public int height;

    public CardSlot[,] cardSlots;
    public Vector2Int cardSpawnPosition;

    public TokenStack[] resourceStacks;
    public int numStacks;
    public int stackCap = 10;

    public Game game;

    public void Initialize(int width, int height, Game game)
    {
        this.game = game;
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
            }
        }

        // Create side board
        InitializeSide();

        // Init start location for cards
        cardSpawnPosition = new Vector2Int(width / 2, height / 2);

        // Debug
        Debug.Log("Created Board: " + cardSlots.GetLength(0) + " x " + cardSlots.GetLength(1));

        // Trigger event
        BoardEvents.instance.TriggerOnInitialize(this);
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
            // Add restriction
            resourceStacks[i].AddWhiteList((TokenType) i);
        }

        this.numStacks = numStacks;
    }

    public void CreateCard(Card card)
    {
        // Create card starting from the card start position
        CreateCard(card, cardSpawnPosition);
    }

    public void CreateCard(Card card, Vector2Int position)
    {
        var cardSlot = GetFirstOpenCardslotFrom(position);

        if (cardSlot != null)
        {
            // Make a copy
            var copy = Instantiate(card);

            // Create card at this slot
            copy.Create(cardSlot);
        }
        else 
        {
            // Debug
            Debug.Log("No open card slots exist.");
        }
    }

    public void CreateToken(ResourceToken token)
    {
        // Loop through stockpile
        foreach (var stack in resourceStacks)
        {
            // Check if token can be added
            if (stack.CanAddToken(token))
            {
                // Create token on that stack
                CreateToken(token, stack);
                // Stop searching
                break;
            }
        }
    }

    public void CreateToken(ResourceToken token, TokenStack stack)
    {
        // Make a copy
        var copy = Instantiate(token);
        // Create the token
        copy.Create(stack);
    }

    private CardSlot GetFirstOpenCardslotFrom(Vector2Int position)
    {
        // Get card slot at that position
        var cardSlot = cardSlots[position.x, position.y];

        Vector2Int origin = position;

        // Loop until a free slot is found
        while (cardSlot.IsOccupied())
        {
            // Shift position to the left
            position.x += 1;

            // Check bounds
            if (position.x >= width)
            {
                // Reset x
                position.x = 0;

                // Shift down by 1
                position.y -= 1;
            }

            if (position.y < 0)
            {
                // Reset y
                position.y = height - 1;
            }

            // Check if we've looped back
            if (position == origin) return null;

            // Update cardslot
            cardSlot = cardSlots[position.x, position.y];
        }

        return cardSlot;
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
