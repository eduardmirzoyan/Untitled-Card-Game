using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cost
{
    public int foodCost;
    public int faithCost;
    public int manpowerCost;
    public int goldCost;

    public Card cardCost;

    public bool IsSatisfied(Card card, TokenStack stack)
    {
        int food = 0, faith = 0, manpower = 0, gold = 0;
        // Check to see if tokens are satified
        foreach (var token in stack.tokens)
        {
            switch (token.tokenType)
            {
                case TokenType.Food:
                    food++;
                    break;
                case TokenType.Faith:
                    faith++;
                    break;
                case TokenType.Manpower:
                    manpower++;
                    break;
                case TokenType.Gold:
                    gold++;
                    break;
                default:
                    Debug.Log("THIS TOKEN TYPE WAS NOT IMPLEMENTED");
                    break;
            }
        }

        bool tokensSatified = food >= foodCost && faith >= faithCost 
                            && manpower >= manpowerCost && gold >= goldCost;

        bool cardSatified = true;
        if (cardCost != null)
        {
            // Make sure the card is the sames
            cardSatified = cardCost != card;    
        }

        // Return final result
        return tokensSatified && cardSatified;
    }
}