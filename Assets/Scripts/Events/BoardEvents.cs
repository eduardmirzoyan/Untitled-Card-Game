using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardEvents : MonoBehaviour
{
    public static BoardEvents instance;
    private void Awake()
    {
        // Singleton Logic
        if (BoardEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public event Action<Board> onInitialize;
    public event Action<Card, Vector2Int> onCreateCard;
    public event Action<Card, Vector2Int, Vector2Int> onMoveCard;

    public void TriggerOnInitalize(Board board)
    {
        if (onInitialize != null)
        {
            onInitialize(board);
        }
    }

    public void TriggerOnCreateCard(Card card, Vector2Int position)
    {
        if (onCreateCard != null)
        {
            onCreateCard(card, position);
        }
    }

    public void TriggerOnMoveCard(Card card, Vector2Int oldPosition, Vector2Int newPosition)
    {
        if (onMoveCard != null)
        {
            onMoveCard(card, oldPosition, newPosition);
        }
    }
}
