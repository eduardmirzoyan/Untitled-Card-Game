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

        // Debug
        Debug.Log("Created: " + this.ToString());
    }

    public bool IsOccupied()
    {
        return card != null;
    }

    public override string ToString()
    {
        return "Card Slot [" + position.x + ", " + position.y + "]";
    }
}
