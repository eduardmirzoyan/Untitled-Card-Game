using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Game : ScriptableObject
{
    public Board board;
    public TokenTable tokenTable;

    public void Initialize(int boardWidth, int boardHeight)
    {
        // Create token table
        tokenTable = ScriptableObject.CreateInstance<TokenTable>();
        tokenTable.Initialize();

        // Create playing board
        board = ScriptableObject.CreateInstance<Board>();
        board.Initialize(boardWidth, boardHeight);
    }
}
