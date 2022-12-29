using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SideBoard : ScriptableObject
{
    public TokenStack[] stacks;
    public int numStacks;
    public int stackCap = 10;

    public void Initialize()
    {
        int numStacks = System.Enum.GetValues(typeof(TokenType)).Length;

        // Create new list
        stacks = new TokenStack[numStacks];

        // Initialize stacks
        for (int i = 0; i < stacks.Length; i++)
        {
            // Create SO
            stacks[i] = ScriptableObject.CreateInstance<TokenStack>();
            // Initialize
            stacks[i].Initialize(stackCap);
        }

        this.numStacks = numStacks;

        // Trigger event
        BoardEvents.instance.TriggerOnInitalizeSide(this);
    }
}
