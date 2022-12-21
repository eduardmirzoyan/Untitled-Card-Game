using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform boardTransform;
    [SerializeField] private Transform sideBoardTransform;
    [SerializeField] private Transform cardSlotsTransform;
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


    public static BoardHandler instance;
    private void Awake()
    {
        // Singleton Logic
        if (BoardHandler.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        // Sub to events
        BoardEvents.instance.onInitialize += Initialize;
    }

    private void OnDestroy()
    {
        // Unsub
        BoardEvents.instance.onInitialize -= Initialize;
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

        // Need to move this to sideboard handler...

        // Make storage at the top of table with height of 1 card
        var storageSize = new Vector3(gapSize.x + (gapSize.x + cardSize.x) * board.width, tableThickness, gapSize.y + (gapSize.y + cardSize.y) * 1);
        // Set size
        sideBoardTransform.localScale = storageSize;
        // Set position
        sideBoardTransform.position = new Vector3(0, 0, storageSize.z / 2 + tableSize.z / 2);

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
        cardSlotHandler.Initialize(cardSlot, worldPosition);
    }

    public Vector3 GetWorldFromCell(Vector2Int position)
    {
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
