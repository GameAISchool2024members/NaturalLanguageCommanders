using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    public BFSearch search;

    // Update is called once per frame
    void Update()
    {
        Vector3Int position = search.ground.WorldToCell(transform.position);
        Vector3Int? updatedPosition =  search.GetNext(position);
        if (updatedPosition != null) {
            transform.position = search.ground.CellToWorld(updatedPosition.Value);
            }
    }
}
