using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Grid grid;
    [SerializeField] private Transform boardTransform;
    [SerializeField] private Transform storageTransform;
    [SerializeField] private GameObject cardPrefab;

    [Header("Settings")]
    [SerializeField] private float tableThickness = 0.25f;
    [SerializeField] private Vector2 cardSize;
    [SerializeField] private Vector2 gapSize;

    [Header("Data")]
    [SerializeField] private Board board;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;

    private void Start()
    {
        // Sub to events
        BoardEvents.instance.onInitialize += Initialize;
        BoardEvents.instance.onCreateCard += CreateCard;
        BoardEvents.instance.onMoveCard += MoveCard;
    }

    private void OnDestroy()
    {
        // Unsub
        BoardEvents.instance.onInitialize -= Initialize;
        BoardEvents.instance.onCreateCard -= CreateCard;
        BoardEvents.instance.onMoveCard -= MoveCard;
    }

    private void Update()
    {
        if (debugMode && board != null)
        {
            if (Input.GetKeyDown(KeyCode.L))
                print(board.ToStringSimple());
        }
    }

    public void Initialize(Board board)
    {
        this.board = board;

        // Set size based on card size and gap
        var tableSize = new Vector3(gapSize.x + (gapSize.x + cardSize.x) * board.width, tableThickness, gapSize.y + (gapSize.y + cardSize.y) * board.height);
        // Set size
        boardTransform.localScale = tableSize;
        // Set position
        boardTransform.position = Vector3.zero;

        // Make storage at the top of table with height of 1 card
        var storageSize = new Vector3(gapSize.x + (gapSize.x + cardSize.x) * board.width, tableThickness, gapSize.y + (gapSize.y + cardSize.y) * 1);
        // Set size
        storageTransform.localScale = storageSize;
        // Set position
        storageTransform.position = new Vector3(0, 0, storageSize.z / 2 + tableSize.z / 2);
    }

    public void CreateCard(Card card, Vector2Int position)
    {
        // Get the card's world position
        var cellPosition = new Vector3Int(position.x, position.y, 0);
        var cellGapOffset = new Vector3(grid.cellGap.x / 2, 0, grid.cellGap.y / 2);
        var worldPosition = grid.GetCellCenterWorld(cellPosition) - cellGapOffset;

        // Create the card object
        var cardHandler = Instantiate(cardPrefab).GetComponent<CardHandler>();
        cardHandler.Initialize(card, worldPosition);
    }

    public void MoveCard(Card card, Vector2Int oldPosition, Vector2Int newPosition)
    {
        // TODO ?
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

    public Vector2Int GetNearestSlot(Vector3 position)
    {
        var cellPositon = grid.WorldToCell(position);
        return (Vector2Int)cellPositon;
    }
}
