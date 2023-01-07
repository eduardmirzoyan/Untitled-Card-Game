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
        GenerateTokens(stack, card.cardSlot.board.game);

        // Create card rewards
        GenerateCards(card.cardSlot);

        // Debug
        // Debug.Log("Rewards yielded");
    }


    private void GenerateTokens(TokenStack stack, Game game)
    {
        for (int i = 0; i < foodReward; i++)
        {
            // Get token from table
            ResourceToken foodToken = game.tokenTable.GetToken(TokenType.Food);

            // Create token
            game.board.CreateToken(foodToken, stack);
        }

        for (int i = 0; i < faithReward; i++)
        {
            // Get token from table
            ResourceToken faithToken = game.tokenTable.GetToken(TokenType.Faith);

            // Create token
            game.board.CreateToken(faithToken, stack);
        }

        for (int i = 0; i < manpowerReward; i++)
        {
            // Get token from table
            ResourceToken mpToken = game.tokenTable.GetToken(TokenType.Manpower);

            // Create token
            game.board.CreateToken(mpToken, stack);
        }

        for (int i = 0; i < goldReward; i++)
        {

            // Get token from table
            ResourceToken goldToken = game.tokenTable.GetToken(TokenType.Gold);

            // Create token
            game.board.CreateToken(goldToken, stack);
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
