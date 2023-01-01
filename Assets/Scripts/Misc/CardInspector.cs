using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInspector : MonoBehaviour
{

    public static CardInspector instance;
    private void Awake()
    {
        // Singleton Logic
        if (CardInspector.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    [Header("Components")]
    [SerializeField] private Transform inspectOrientation;

    [Header("Data")]
    [SerializeField] private CardHandler cardHandler;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Quaternion originalRotation;
    [SerializeField] private bool canInteractWith;

    [Header("Settings")]
    [SerializeField] private float inspectSpeed = 1f;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private float counterRotateSpeed = 1f;

    private Coroutine routine;

    public void Inspect(CardHandler cardHandler)
    {
        // Check if we are already inspecting a card
        if (this.cardHandler != null)
        {
            // Return previous card
            // StartCoroutine(ReturnCardToBoard(inspectSpeed));
        }

        // Save ref
        this.cardHandler = cardHandler;
        // Save orientation
        originalPosition = cardHandler.transform.position;
        originalRotation = cardHandler.transform.rotation;

        // Start inspect routine
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(BringCardToCamera(inspectSpeed));
    }

    private void Return()
    {
        // Start routine
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ReturnCardToBoard(inspectSpeed));
    }

    private IEnumerator BringCardToCamera(float duration)
    {
        // Disable physics
        cardHandler.EnablePhysics(false);

        // Set values
        Vector3 startPosition = cardHandler.transform.position;
        Vector3 endPosition = inspectOrientation.position;

        Quaternion startRotation = cardHandler.transform.rotation;
        Quaternion endRotation = inspectOrientation.rotation;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp position of card towards cam
            cardHandler.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);

            // Lerp rotation of card to face camera
            cardHandler.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);

            // Debug
            // print(elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final location
        cardHandler.transform.position = endPosition;

        // Set final rotation
        cardHandler.transform.rotation = endRotation;

        // Change state
        canInteractWith = true;
    }

    private IEnumerator ReturnCardToBoard(float duration)
    {
        // Set values
        Vector3 startPosition = cardHandler.transform.position;
        Vector3 endPosition = originalPosition;

        Quaternion startRotation = cardHandler.transform.rotation;
        Quaternion endRotation = originalRotation;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp position of card towards cam
            cardHandler.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);

            // Lerp rotation of card to face camera
            cardHandler.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);

            // Debug
            // print(elapsed / duration);

            // Increment
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final location
        cardHandler.transform.position = endPosition;

        // Set final rotation
        cardHandler.transform.rotation = endRotation;

        // Change state
        canInteractWith = false;

        // Return physics
        cardHandler.EnablePhysics(true);

        // Remove ref
        cardHandler = null;
    }

    private void Update()
    {
        // If a card is ready to be moved
        if (canInteractWith)
        {
            // Right click
            if (Input.GetMouseButtonDown(1))
            {
                // Return the card immediately
                Return();
            }

            // Left click
            if (Input.GetMouseButton(0))
            {
                float xRotation = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
                float yRotation = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;

                cardHandler.transform.Rotate(Vector3.forward, -xRotation);
                cardHandler.transform.Rotate(Vector3.right, yRotation);
            }
            // If the player isn't interacting
            else
            {
                // Try to rotate back to neutral
                cardHandler.transform.rotation = Quaternion.RotateTowards(cardHandler.transform.rotation, inspectOrientation.rotation, counterRotateSpeed);
            }
        }

        
    }
}
