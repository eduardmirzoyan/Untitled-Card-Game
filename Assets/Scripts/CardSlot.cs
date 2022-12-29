using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : ScriptableObject
{
    [Header("Data")]
    public Vector2Int position;
    public Card card;

    [Header("Parent")]
    public Board board;

    public void Initialize(Vector2Int position, Board board)
    {
        this.position = position;
        this.board = board;
        this.card = null;
    }

    public void SetCard(Card card)
    {
        this.card = card;
        if (card != null) card.cardSlot = this;
    }

    public bool IsOccupied()
    {
        return card != null;
    }

    public override string ToString()
    {
        var cardName = card != null ? card.name : "None";
        return "{ " + position.x + " , " + position.y + " } w/ Card: " + cardName;
    }
}
