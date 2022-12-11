using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotHandler : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CardSlot cardSlot;

    private void Start()
    {
        // Sub to events
        // TODO
    }

    private void OnDestroy()
    {
        // Unsub
        // TODO
    }

    public void Initialize(CardSlot cardSlot)
    {
        this.cardSlot = cardSlot;

        // TODO
    }
}
