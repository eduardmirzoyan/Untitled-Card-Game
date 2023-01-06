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

    public void TriggerOnInitialize(Board board)
    {
        if (onInitialize != null)
        {
            onInitialize(board);
        }
    }
}
