using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimersUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image lifetimeTimerImage;
    [SerializeField] private Image effectTimerImage;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // Always face camera
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
