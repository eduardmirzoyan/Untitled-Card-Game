using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Board : ScriptableObject
{
    public int height;
    public int width;

    public void Initialize(int height, int width)
    {
        this.height = height;
        this.width = width;
    }

    
}
