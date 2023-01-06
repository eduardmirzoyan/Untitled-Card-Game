using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward
{
    [Header("Tokens")]
    public int foodReward;
    public int faithReward;
    public int manpowerReward;
    public int goldReward;

    [Header("Cards")]
    public Card[] cardRewards;
    public bool randomlySelect;

    public void Generate(Card card, TokenStack stack)
    {
        // Create token rewards
        GenerateTokens(stack);

        // Create card rewards
        GenerateCards(card.cardSlot);

        // Debug
        // Debug.Log("Rewards yielded");
    }


    private void GenerateTokens(TokenStack stack)
    {
        for (int i = 0; i < foodReward; i++)
        {
            // Get Create token
            ResourceToken foodToken = new ResourceToken();

            // Add them to stack
            foodToken.MoveTo(stack);
        }

        for (int i = 0; i < faithReward; i++)
        {
            // Get Create token
            ResourceToken faithToken = new ResourceToken();

            // Add them to stack
            faithToken.MoveTo(stack);
        }

        for (int i = 0; i < manpowerReward; i++)
        {
            // Get Create token
            ResourceToken mpToken = new ResourceToken();

            // Add them to stack
            mpToken.MoveTo(stack);
        }

        for (int i = 0; i < goldReward; i++)
        {
            
            // Get Create token
            ResourceToken goldToken = new ResourceToken();

            // Add them to stack
            goldToken.MoveTo(stack);
        }
    }

    private void GenerateCards(CardSlot cardSlot)
    {   
        // If no card rewards, then don't do anything
        if (cardRewards.Length == 0) return;

        if (randomlySelect)
        {
            // Randomly choose 1 card from all possible
            var card = cardRewards[Random.Range(0, cardRewards.Length)];
            // Create the card
            cardSlot.board.CreateCard(card);
        }
        else 
        {
            // Generate ALL the card rewards
            foreach (var card in cardRewards)
            {
                // Create the card
                cardSlot.board.CreateCard(card);
            }
        }
    }
}
