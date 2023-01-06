using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera cam;

    [Header("Settings")]
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float zoomSpeed = 10f;

    private void Awake()
    {
        // Cache ref
        cam = Camera.main;
    }

    private void Update()
    {
        // TODO
    }
}
