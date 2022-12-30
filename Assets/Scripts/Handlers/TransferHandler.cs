using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<TokenHandler> tokenHandlers;

    [Header("Settings")]
    [SerializeField] private float pickUpHeight = 0.5f;

    public static TransferHandler instance;
    private void Awake()
    {
        // Singleton Logic
        if (TransferHandler.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Init list
        tokenHandlers = new List<TokenHandler>();
    }

    private void Update()
    {
        // Always follow mouse
        FollowMouse();
    }

    public void AddToTransport(Transform transform)
    {
        // Set the parent to this
        transform.parent = this.transform;
    }

    public void ClearTransport()
    {
        // TODO
    }

    private void FollowMouse()
    {
        // Create ground plane
        Plane plane = new Plane(Vector3.up, Vector3.up * pickUpHeight);

        // Create a ray from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance)) // the distance from the ray origin to the ray intersection of the plane
        {
            transform.position = ray.GetPoint(distance); // distance along the ray
        }
    }
}
