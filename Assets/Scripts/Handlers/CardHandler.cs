using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using TMPro;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StackHandler inputStack;
    [SerializeField] private StackHandler outputStack;
    [SerializeField] private cakeslice.Outline outline;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Collider hitbox;
    [SerializeField] private TextMeshPro topText;
    [SerializeField] private TextMeshPro middleText;
    [SerializeField] private TextMeshPro bottomText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TimersUI timersUI;

    [Header("Data")]
    [SerializeField] private Card card;
    [SerializeField] private Vector3 homePosition;
    [SerializeField] private bool isInteractable = true;

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

        // Initialize visuals
        middleText.text = card.name;

        // Disable outline
        DisableOutline();

        // Set home
        Relocate(homeTransform);

        // Initialize stacks
        inputStack.Initialize(card.inputStack);
        outputStack.Initialize(card.outputStack);

        // Init timers
        timersUI.Initialize(card);

        // Update name
        name = this.ToString();
    }

    private void Update()
    {
        // If paused, then don't tick
        if (GameHandler.instance.isPaused) return;

        // If a card exits and is satified, start to tick it down
        if (card != null)
        {
            // Tick timer every frame
            card.TickTimers();
        }
    }

    private void OnMouseEnter()
    {
        if (!isInteractable) return;

        EnableOutline();

        // Trigger event
        CardEvents.instance.TriggerOnDrag(card, true);
    }

    private void OnMouseExit()
    {
        if (!isInteractable) return;

        DisableOutline();

        // Trigger event
        CardEvents.instance.TriggerOnDrag(card, false);
    }

    private void OnMouseDrag()
    {
        if (!isInteractable) return;

        // Follow the mouse while slighly hovering over the board
        FollowMouse();

        // Check for card handlers under
        CheckForSlot();
    }

    private void OnMouseUp()
    {
        if (!isInteractable) return;

        // Move to the new position you are hovering
        MoveCard();

        // Return card to home position
        transform.position = homePosition;
    }

    private void OnMouseOver()
    {
        if (!isInteractable) return;

        // Check for Right Click
        if (Input.GetMouseButtonDown(1))
        {
            // Debug
            // print("Right clicked on Card: " + card.ToString());

            // Inspect this card
            CardInspector.instance.Inspect(this);

            // Trigger event
            CardEvents.instance.TriggerOnInspect(card);
        }
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
        // Create a ray from mouse position towards the board
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, layerMask);

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

    public void EnablePhysics(bool state)
    {
        // Toggle physics
        body.useGravity = state;

        // Toggle collisions
        hitbox.enabled = state;

        // Toggle interactibility
        isInteractable = state;

        // Toggle UI
        canvas.enabled = state;
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
