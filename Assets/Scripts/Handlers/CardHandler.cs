using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class CardHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StackHandler inputStack;
    [SerializeField] private StackHandler outputStack;
    [SerializeField] private Vector3 homePosition;
    [SerializeField] private Outline outline;

    [Header("Data")]
    [SerializeField] private Card card;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;

    private CardSlotHandler cardSlotHandler;

    private void Start()
    {
        // Sub
        CardEvents.instance.onDestroy += DestroyCard;
    }

    private void OnDestroy()
    {
        // Unsub
        CardEvents.instance.onDestroy -= DestroyCard;
    }

    public void Initialize(Card card, Transform homeTransform)
    {
        this.card = card;
        transform.position = homeTransform.position;

        // Disable outline
        DisableOutline();

        // Set home
        Relocate(homeTransform);

        // Initialize stacks
        inputStack.Initialize(card.inputStack);
        outputStack.Initialize(card.outputStack);

        // Update name
        name = this.ToString();
    }

    private void OnMouseEnter()
    {
        EnableOutline();

        // Trigger event
        CardEvents.instance.TriggerOnDrag(card, true);
    }

    private void OnMouseExit()
    {
        DisableOutline();

        // Trigger event
        CardEvents.instance.TriggerOnDrag(card, false);
    }

    private void OnMouseDrag()
    {
        // Follow the mouse while slighly hovering over the board
        FollowMouse();

        // Check for card handlers under
        CheckForSlot();
    }

    private void OnMouseUp()
    {
        // Move to the new position you are hovering
        MoveCard();

        // Return card to home position
        transform.position = homePosition;
    }

    private void FollowMouse()
    {
        // Create ground plane
        Plane plane = new Plane(Vector3.up, Vector3.up * pickUpHeight);

        // Create a ray from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance; // the distance from the ray origin to the ray intersection of the plane
        if (plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance); // distance along the ray
        }
    }

    private void CheckForSlot()
    {
        // Raycast down from the mouse
        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 10f, layerMask);

        // Check to see if you hit a card slot
        if (hit)
        {
            if (hitInfo.transform.parent.TryGetComponent(out CardSlotHandler cardSlotHandler))
            {
                // Do nothing if same
                if (this.cardSlotHandler == cardSlotHandler) return;

                // Else check all 4 possibilities
                if (this.cardSlotHandler != null && cardSlotHandler != null)
                {
                    // Disable old
                    CardEvents.instance.TriggerOnDrop(this.cardSlotHandler.GetCardSlot(), false);

                    // Enable new
                    CardEvents.instance.TriggerOnDrop(cardSlotHandler.GetCardSlot(), true);
                }
                else if (this.cardSlotHandler == null && cardSlotHandler != null)
                {
                    // Enable new
                    CardEvents.instance.TriggerOnDrop(cardSlotHandler.GetCardSlot(), true);
                }
                else if (this.cardSlotHandler != null && cardSlotHandler == null)
                {
                    // Disable old
                    CardEvents.instance.TriggerOnDrop(this.cardSlotHandler.GetCardSlot(), false);
                }
                else if (this.cardSlotHandler == null && cardSlotHandler == null)
                {
                    // Nothing lol
                }

                // Update slot
                this.cardSlotHandler = cardSlotHandler;
            }
        }
    }

    private void MoveCard()
    {
        // If you've found a slot
        if (cardSlotHandler != null)
        {
            // Move token to new stack
            var slot = cardSlotHandler.GetCardSlot();
            var moved = card.MoveTo(slot);

            // Check if move was sucessful
            if (moved)
            {
                // Debug
                // print(ToString() + " Relocated to " + cardSlotHandler.ToString());

                // Get new location
                var newParent = cardSlotHandler.GetCardTransform();

                // Relocation token
                Relocate(newParent);
            }

            // Trigger event
            CardEvents.instance.TriggerOnDrop(slot, false);

            // Reset
            cardSlotHandler = null;
        }
    }

    private void DestroyCard(Card card)
    {
        if (this.card != card) return;

        // Destroy self
        Destroy(gameObject);
    }

    private void Relocate(Transform parent)
    {
        // Change parent
        // transform.parent = parent;

        // Set home
        homePosition = parent.position;

        // Move to home
        transform.position = homePosition;
    }

    private void EnableOutline()
    {
        outline.eraseRenderer = false;
    }

    private void DisableOutline()
    {
        outline.eraseRenderer = true;
    }

    public override string ToString()
    {
        return card.name + " Object";
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
    }
}
