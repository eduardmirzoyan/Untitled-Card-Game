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

    [Header("Data")]
    [SerializeField] private Card card;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnDestroy()
    {
        // Unsub
        CardEvents.instance.onTickLife -= UpdateLife;
        CardEvents.instance.onTickUse -= UpdateUse;
    }

    public void Initialize(Card card)
    {
        this.card = card;

        // Sub
        CardEvents.instance.onTickLife += UpdateLife;
        CardEvents.instance.onTickUse += UpdateUse;
    }

    private void Update()
    {
        // Always face camera
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

    private void UpdateLife(Card card)
    {
        // Update ratio
        lifetimeTimerImage.fillAmount = card.lifeCounter / card.lifetime;
    }

    private void UpdateUse(Card card)
    {
        // Update ratio
        effectTimerImage.fillAmount = card.useCounter / card.usetime;
    }
}
