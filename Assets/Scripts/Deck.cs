using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : ScriptableObject
{
    public List<Card> cards;

    public void Initialize()
    {
        // Initialize list
        cards = new List<Card>();
    }
}
