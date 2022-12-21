using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SideBoard : ScriptableObject
{
    public TokenStack goldTokenStack;
    public TokenStack foodTokenStack;
    public TokenStack faithTokenStack;
    public TokenStack manpowerTokenStack;

    public void Initialize()
    {
        // Initialize stacks
        goldTokenStack = ScriptableObject.CreateInstance<TokenStack>();
        goldTokenStack.Initialize(10);

        foodTokenStack = ScriptableObject.CreateInstance<TokenStack>();
        foodTokenStack.Initialize(10);

        faithTokenStack = ScriptableObject.CreateInstance<TokenStack>();
        faithTokenStack.Initialize(10);

        manpowerTokenStack = ScriptableObject.CreateInstance<TokenStack>();
        manpowerTokenStack.Initialize(10);

        // Trigger event
        BoardEvents.instance.TriggerOnInitalizeSide(this);
    }
}
