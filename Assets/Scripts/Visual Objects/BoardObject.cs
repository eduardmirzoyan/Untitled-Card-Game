using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardObject : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Grid grid;

    public Vector3 GetNearestGridPosition(Vector3 position)
    {
        var cell = grid.WorldToCell(position);
        
        return grid.GetCellCenterWorld(cell);
    }
}
