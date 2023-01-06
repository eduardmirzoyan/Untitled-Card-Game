using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimersUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image lifetimeTimerImage;
    [SerializeField] private Image effectTimerImage;
    [SerializeField] private TextMeshProUGUI usesText;

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
        CardEvents.instance.onTickLife -= UpdateLifeTimer;
        CardEvents.instance.onTickUse -= UpdateUseTimer; 
        CardEvents.instance.onTrigger -= UpdateUsesText;
        CardEvents.instance.onInspect -= Hide;
        CardEvents.instance.onUninspect -= Show;
    }

    public void Initialize(Card card)
    {
        this.card = card;

        if (card.numUses == -1)
        {
            // Set to inf char
            usesText.text = "\u221E";
        }
        else
        {
            usesText.text = "" + card.numUses;
        }

        // Sub
        CardEvents.instance.onTickLife += UpdateLifeTimer;
        CardEvents.instance.onTickUse += UpdateUseTimer;
        CardEvents.instance.onTrigger += UpdateUsesText;
        CardEvents.instance.onInspect += Hide;
        CardEvents.instance.onUninspect += Show;
    }

    private void Update()
    {
        // Always face camera
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

    private void Show(Card card)
    {
        if (this.card != card) return;

        // Show UI
        canvasGroup.alpha = 1f;
    }

    private void Hide(Card card)
    {
        if (this.card != card) return;

        // Hide UI
        canvasGroup.alpha = 0f;
    }

    private void SetTransparent(Card card)
    {
        if (this.card != card) return;

        // Transparent UI
        canvasGroup.alpha = 0.4f;
    }

    private void UpdateUsesText(Card card)
    {
        if (this.card != card) return;

        // Update text
        if (card.numUses == -1)
        {
            // Set to inf char
            usesText.text = "\u221E";
        }
        else
        {
            usesText.text = "" + card.numUses;
        }
    }

    private void UpdateLifeTimer(Card card)
    {
        if (this.card != card) return;

        // Update ratio
        lifetimeTimerImage.fillAmount = card.lifeCounter / card.lifetime;
    }

    private void UpdateUseTimer(Card card)
    {
        if (this.card != card) return;

        // Update ratio
        effectTimerImage.fillAmount = card.useCounter / card.usetime;
    }
}
