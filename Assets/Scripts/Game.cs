using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Game : ScriptableObject
{
    public Board board;
    public Inventory inventory;

    public void Initialize(int boardWidth, int boardHeight)
    {
        board = ScriptableObject.CreateInstance<Board>();
        board.Initialize(boardWidth, boardHeight);

        inventory = ScriptableObject.CreateInstance<Inventory>();
        inventory.Initialize();
    }
}
