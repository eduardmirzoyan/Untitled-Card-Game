using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Game : ScriptableObject
{
    public Board board;

    public void Initialize(int boardWidth, int boardHeight)
    {
        board = ScriptableObject.CreateInstance<Board>();
        board.Initialize(boardWidth, boardHeight);
    }
}
