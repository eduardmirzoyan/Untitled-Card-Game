using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public BoardHandler boardHandler;
    [SerializeField] private GameObject cardPrefab;

    [Header("Data")]
    [SerializeField] private CardSlot cardSlot;

    private void Start()
    {
        // Sub to events
        BoardEvents.instance.onCreateCard += CreateCard;
    }

    private void OnDestroy()
    {
        // Unsub
        BoardEvents.instance.onCreateCard -= CreateCard;
    }

    public void Initialize(CardSlot cardSlot, Vector3 position, BoardHandler boardHandler)
    {
        this.cardSlot = cardSlot;
        transform.position = position;
        this.boardHandler = boardHandler;
    }

    public void CreateCard(Card card, Vector2Int position)
    {
        if (cardSlot.position != position) return;

        // Get the card's world position
        var worldPosition = boardHandler.GetWorldFromCell(position);

        // Create the card object
        var cardHandler = Instantiate(cardPrefab).GetComponent<CardHandler>();
        cardHandler.Initialize(card, worldPosition, this);
    }

    public CardSlot GetCardSlot()
    {
        return cardSlot;
    }

    public new string ToString()
    {
        return cardSlot.ToString();
    }
}
