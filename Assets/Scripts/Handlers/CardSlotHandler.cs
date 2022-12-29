using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardDropTransform;

    [Header("Data")]
    [SerializeField] private CardSlot cardSlot;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private float debugRadius = 0.1f;

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

    public void Initialize(CardSlot cardSlot, Vector3 position)
    {
        this.cardSlot = cardSlot;
        transform.position = position;
        
        // Update name
        name = this.ToString();
    }

    private void CreateCard(Card card, Vector2Int position)
    {
        if (cardSlot.position != position) return;

        // Create the card object
        var cardHandler = Instantiate(cardPrefab).GetComponent<CardHandler>();
        cardHandler.Initialize(card, cardDropTransform);
    }

    public Transform GetCardTransform()
    {
        return cardDropTransform;
    }
    
    public CardSlot GetCardSlot()
    {
        return cardSlot;
    }

    public override string ToString()
    {
        return cardSlot.ToString();
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cardDropTransform.position, debugRadius);
    }
}
