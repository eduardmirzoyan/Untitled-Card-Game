using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBoardHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StackHandler[] stackHandlers;

    [Header("Settings")]
    [SerializeField] private float tableThickness = 0.25f;
    [SerializeField] private Vector2 cardSize;
    [SerializeField] private Vector2 gapSize;

    [Header("Data")]
    [SerializeField] private SideBoard sideBoard;

    private void Start()
    {
        // Sub to events
        BoardEvents.instance.onInitializeSide += Initialize;
    }

    private void OnDestroy()
    {
        // Unsub
        BoardEvents.instance.onInitializeSide -= Initialize;
    }

    public void Initialize(SideBoard sideBoard)
    {
        this.sideBoard = sideBoard;

        // Scale size of side board
        // CURRENTLY HANDLED BY THE BOARD HANDLER NEED TO CHANGE

        // Position stack locations
        float segment = transform.localScale.x / (sideBoard.numStacks + 1);

        Vector3 offset = new Vector3(transform.localScale.x / 2, 0, 0);

        for (int i = 0; i < sideBoard.numStacks; i++)
        {
            // Calculate position
            Vector3 position = new Vector3((i + 1) * segment, transform.position.y, transform.position.z) - offset;
            stackHandlers[i].transform.position = position;

            // Initialize stacks
            stackHandlers[i].Initialize(sideBoard.stacks[i]);

            // Create stack
            //var stack = Instantiate(stackPrefab, position, Quaternion.identity, transform);
        }
    }
}
