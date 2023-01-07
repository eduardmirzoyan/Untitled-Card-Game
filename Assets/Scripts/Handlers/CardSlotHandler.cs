using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardDropTransform;
    [SerializeField] private MeshRenderer meshRenderer;

    [Header("Data")]
    [SerializeField] private CardSlot cardSlot;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;
    [SerializeField] private float debugRadius = 0.1f;

    private void Start()
    {
        // Sub to events
        CardEvents.instance.onCreate += CreateCard;
        CardEvents.instance.onDrop += ToggleHighlight;
    }

    private void OnDestroy()
    {
        // Unsub
        CardEvents.instance.onCreate -= CreateCard;
        CardEvents.instance.onDrop -= ToggleHighlight;
    }

    public void Initialize(CardSlot cardSlot, Vector3 position)
    {
        this.cardSlot = cardSlot;
        transform.position = position;
        
        // Update name
        name = this.ToString();
    }

    private void CreateCard(Card card, CardSlot cardSlot)
    {
        // Make sure it's this slot
        if (this.cardSlot != cardSlot) return;

        // Create the card object
        var cardHandler = Instantiate(cardPrefab).GetComponent<CardHandler>();
        cardHandler.Initialize(card, cardDropTransform);
    }

    private void ToggleHighlight(CardSlot cardSlot, bool state)
    {
        // Make sure it's this slot
        if (this.cardSlot != cardSlot) return;

        if (state)
        {
            // Highlight this slot
            EnableHighlight();
        }
        else
        {
            // Remove highlight
            DisableHighlight();
        }
    }

    private void EnableHighlight()
    {
        // Show mesh
        meshRenderer.enabled = true;
    }

    private void DisableHighlight()
    {
        // Hide mesh
        meshRenderer.enabled = false;
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
        return cardSlot.ToString() + " Object";
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cardDropTransform.position, debugRadius);
    }
}
