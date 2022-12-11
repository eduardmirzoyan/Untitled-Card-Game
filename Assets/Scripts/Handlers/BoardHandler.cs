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
    [SerializeField] private Transform cardSlotsTransform;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardSlotPrefab;

    [Header("Settings")]
    [SerializeField] private float tableThickness = 0.25f;
    [SerializeField] private Vector2 cardSize;
    [SerializeField] private Vector2 gapSize;

    [Header("Data")]
    [SerializeField] private Board board;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;

    private Vector3 tableOffset;

    private void Start()
    {
        // Sub to events
        BoardEvents.instance.onInitialize += Initialize;
        BoardEvents.instance.onMoveCard += MoveCard;
    }

    private void OnDestroy()
    {
        // Unsub
        BoardEvents.instance.onInitialize -= Initialize;
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
        boardTransform.position = Vector3.zero; // new Vector3(tableSize.x / 2, 0, tableSize.z / 2);

        // Set offset to be used in other calculations
        tableOffset = new Vector3(tableSize.x / 2, 0, tableSize.z / 2);

        // Make storage at the top of table with height of 1 card
        var storageSize = new Vector3(gapSize.x + (gapSize.x + cardSize.x) * board.width, tableThickness, gapSize.y + (gapSize.y + cardSize.y) * 1);
        // Set size
        storageTransform.localScale = storageSize;
        // Set position
        storageTransform.position = new Vector3(0, 0, storageSize.z / 2 + tableSize.z / 2);

        // Create card slots
        foreach (var cardSlot in board.cardSlots)
        {
            // Create the slot object
            CreateCardSlot(cardSlot);
        }
    }

    private void CreateCardSlot(CardSlot cardSlot)
    {
        // Get the world position
        var worldPosition = GetWorldFromCell(cardSlot.position);

        // Create slot object as child
        var cardSlotHandler = Instantiate(cardSlotPrefab, cardSlotsTransform).GetComponent<CardSlotHandler>();
        cardSlotHandler.Initialize(cardSlot, worldPosition, this);
    }

    // public void CreateCard(Card card, Vector2Int position)
    // {
    //     // Get the card's world position
    //     var worldPosition = GetWorldFromCell(position);

    //     // Create the card object
    //     var cardHandler = Instantiate(cardPrefab).GetComponent<CardHandler>();
    //     cardHandler.Initialize(card, worldPosition);
    // }

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

    public Vector3 GetWorldFromCell(Vector2Int position)
    {
        // This method uses the grid
        // var cellPosition = new Vector3Int(position.x, position.y, 0);
        // var cellGapOffset = new Vector3(grid.cellGap.x / 2, 0, grid.cellGap.y / 2);
        // var worldPosition = grid.GetCellCenterWorld(cellPosition) - cellGapOffset;

        var xOffset = gapSize.x * (position.x + 1);
        var yOffset = gapSize.y * (position.y + 1);

        // Manually get position
        float xPos = xOffset + cardSize.x / 2 + position.x * cardSize.x;
        float yPos = yOffset + cardSize.y / 2 + position.y * cardSize.y;

        // Get world position incorperating offset
        var worldPosition = new Vector3(xPos, 0, yPos) - tableOffset;

        return worldPosition;
    }
}
