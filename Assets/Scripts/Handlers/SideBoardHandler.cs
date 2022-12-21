using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBoardHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform[] stackTransforms;
    [SerializeField] private GameObject stackPrefab;

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
        int num = System.Enum.GetValues(typeof(TokenType)).Length;
        float segment = transform.localScale.x / (num + 1);

        Vector3 offset = new Vector3(transform.localScale.x / 2, 0, 0);

        for (int i = 0; i < num; i++)
        {
            // Calculate position
            Vector3 position = new Vector3((i + 1) * segment, transform.position.y, transform.position.z) - offset;
            // Create stack
            //var stack = Instantiate(stackPrefab, position, Quaternion.identity, transform);
            stackTransforms[i].position = position;
        }
    }

    private void ChangeParentScale(Transform parent, Vector3 scale)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            child.parent = null;
            children.Add(child);
        }
        parent.localScale = scale;
        foreach (Transform child in children) child.parent = parent;
    }

}
