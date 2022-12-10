using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardObject : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Grid grid;
    [SerializeField] private Transform boardTransform;
    [SerializeField] private Transform storageTransform;
    
    
    [Header("Settings")]
    [SerializeField] private float tableThickness = 0.25f;
    [SerializeField] private Vector2 cardSize;
    [SerializeField] private Vector2 gapSize;

    [Header("Data")]
    [SerializeField] private Board board;

    private void Start()
    {
        if (board == null) return;

        // Set size based on card size and gap
        var tableSize = new Vector3(gapSize.x + (gapSize.x + cardSize.x) * board.width, tableThickness, gapSize.y + (gapSize.y + cardSize.y) * board.height);
        boardTransform.localScale = tableSize;
        boardTransform.position = Vector3.zero;

        // Make storage at the top of table with height of 1 card
        var storageSize = new Vector3(gapSize.x + (gapSize.x + cardSize.x) * board.width, tableThickness, gapSize.y + (gapSize.y + cardSize.y) * 1);
        storageTransform.localScale = storageSize;
        // Relocate
        storageTransform.position = new Vector3(0, 0, storageSize.z / 2 + tableSize.z / 2);

        // Draw based on board
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.width; j++)
            {
                // TODO
            }
        }
    }
    public Vector3 GetNearestGridPosition(Vector3 position)
    {
        // Debug
        //var p1 = grid.CellToWorld(cell);
        //Debug.DrawRay(p1, Vector3.up, Color.red, 30f);

        // Debug
        // var p2 = grid.GetCellCenterWorld(cell);
        // Debug.DrawRay(p2, Vector3.up, Color.blue, 30f);

        var cellPosition = grid.WorldToCell(position);
        var cellGapOffset = new Vector3(grid.cellGap.x / 2, 0, grid.cellGap.y / 2);
        var worldPosition = grid.GetCellCenterWorld(cellPosition) - cellGapOffset;
        
        // Debug
        Debug.DrawRay(worldPosition, Vector3.up, Color.green, 30f);

        return worldPosition;
    }
}
