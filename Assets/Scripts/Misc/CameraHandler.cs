using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera cam;

    [Header("Settings")]
    [SerializeField] private float panSpeed = 40f;
    [SerializeField] private float zoomSpeed = 10f;

    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 120f;

    void Update()
    {
        // If you hold middle mouse
        if (Input.GetMouseButton(2))
        {
            // Get changes in x and y
            float deltaX = -Input.GetAxis("Mouse X");
            float deltaY = -Input.GetAxis("Mouse Y");

            // Calcualte movement
            Vector3 displacement = new Vector3(deltaX * panSpeed * Time.deltaTime, 0, deltaY * panSpeed * Time.deltaTime);

            // Alter transform
            transform.position += displacement;
        }

        // Reset button
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Reset position
            transform.position = Vector3.zero;
        }
        

        // Vector3 pos = transform.position;

        // if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        // {
        //     pos.z += panSpeed * Time.deltaTime;
        // }

        // if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        // {
        //     pos.z -= panSpeed * Time.deltaTime;
        // }

        // if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        // {
        //     pos.x += panSpeed * Time.deltaTime;
        // }

        // if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        // {
        //     pos.x -= panSpeed * Time.deltaTime;
        // }

        // float scroll = Input.GetAxis("Mouse ScrollWheel");
        // pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        // pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        // pos.y = Mathf.Clamp(pos.y, minY, maxY);
        // pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // transform.position = pos;
    }
}
