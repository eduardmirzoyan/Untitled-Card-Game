using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform tokenInputTransform;
    [SerializeField] private Transform tokenOutputTransform;

    [Header("Data")]
    [SerializeField] private Card card;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private bool debugMode;

    private void Start()
    {
        // Sub
        BoardEvents.instance.onMoveCard += MoveCard;
    }

    private void OnDestroy()
    {
        // Unsub
        BoardEvents.instance.onMoveCard -= MoveCard;
    }

    public void Initialize(Card card, Vector3 position)
    {
        this.card = card;
        transform.position = position;
    }

    private void OnMouseDown()
    {
        // TODO?
    }

    private void OnMouseDrag()
    {
        FollowMouse();
    }

    private void OnMouseUp()
    {
        // Move to the new position you are hovering
        MoveCard();
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
        Vector3 worldPosition = transform.position;
        RaycastHit hitInfo;

        var hit = Physics.Raycast(worldPosition, Vector3.down, out hitInfo, 10f, layerMask);

        // Check to see if you hit a grid
        if (hit)
        {
            if (hitInfo.transform.TryGetComponent(out BoardHandler boardHandler))
            {
                // Get the closest nearby slot position
                var newPosition = boardHandler.GetNearestSlot(worldPosition);

                // Move this card
                card.cardSlot.board.MoveCard(card, card.cardSlot.position, newPosition);

                // Find the nearest slot to snap to
                // var boardPosition = boardHandler.GetNearestGridPosition(worldPosition);

                // Set new position
                // transform.position = boardPosition;
            }

        }
    }

    public void MoveCard(Card card, Vector2Int oldPosition, Vector2Int newPosition)
    {
        // TODO?
    }

    public Vector3 GetTokenInputPosition()
    {
        return tokenInputTransform.position;
    }

    private void OnDrawGizmosSelected()
    {
        // Make sure you are in debug mode
        if (!debugMode) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 10f);
    }
}
