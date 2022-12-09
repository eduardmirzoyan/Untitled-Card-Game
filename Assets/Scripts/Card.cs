using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivePeriod { Day, Night, Both };

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [Header("Explicit Values")]
    public int lifetime; // If == -1, then indefinite
    public ActivePeriod activePeriod;
    public Cost cost;
    public int usesLeft; // If == -1, then indefinite

    [Header("Implicit Values")]
    public bool isActive;

    [Header("Visuals")]
    public new string name;
    public string effectDescription;
    public Sprite sprite;

    public void Initialize()
    {

    }

    public bool SatisfiesCost()
    {
        // TODO
        return false;
    }

    public virtual void ActivateEffect()
    {
        // Perform the actual effect here
        // TODO

        // Check if you have any uses left
        if (usesLeft > 0)
        {
            // Decrement
            usesLeft--;

            if (usesLeft == 0)
            {
                // Destroy this
            }
        }
    }
}
