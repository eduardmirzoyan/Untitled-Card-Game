using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float outlineThickness = 5f;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;

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
        SetHome(homeTransform);

        // Initialize stacks
        inputStack.Initialize(card.inputStack);
        outputStack.Initialize(card.outputStack);

        // Update name
        name = this.ToString();
    }

    private void OnMouseEnter()
    {
        EnableOutline();
    }

    private void OnMouseExit()
    {
        DisableOutline();
    }

    private void OnMouseDrag()
    {
        // Follow the mouse while slighly hovering over the board
        FollowMouse();
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

    private void MoveCard()
    {
        // Raycast down from the mouse
        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 10f, layerMask);

        // Check to see if you hit a card slot
        if (hit)
        {
            if (hitInfo.transform.parent.TryGetComponent(out CardSlotHandler cardSlotHandler))
            {
                // Debug
                // print(cardSlotHandler.ToString());

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
                    SetHome(newParent);
                }
            }
        }
    }

    public void MoveCard(Card card, Vector2Int oldPosition, Vector2Int newPosition)
    {
        // REMOVE THIS?

        if (this.card == card)
        {
            // Get world position from board
            // var worldPosition = BoardHandler.instance.GetWorldFromCell(newPosition);

            // Set new home
            // homePosition = worldPosition;
        }
    }

    private void DestroyCard(Card card)
    {
        if (this.card != card) return;

        // Destroy self
        Destroy(gameObject);
    }

    private void SetHome(Transform parent)
    {
        // Change parent
        // transform.parent = parent;

        // Set home
        homePosition = parent.position;

        // Give a big y offset
        // transform.position += Vector3.up * initialYOffset;
    }

    private void EnableOutline()
    {
        outline.OutlineWidth = outlineThickness;
    }

    private void DisableOutline()
    {
        outline.OutlineWidth = 0f;
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
