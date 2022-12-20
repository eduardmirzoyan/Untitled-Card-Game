using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenType { Food, Faith, Gold, Manpower };

[CreateAssetMenu]
public class ResourceToken : ScriptableObject
{
    public TokenType tokenType;
    public Board board;

    public void Initialize(TokenType tokenType, Board board)
    {
        this.tokenType = tokenType;
        this.board = board;
    }

    public void Destroy()
    {
        this.board = null;

        // Trigger event
        TokenEvents.instance.TriggerOnDestroy(this);
    }
}
