using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    public AStarSearch search;
    public Tilemap ground;

    private void Start()
    {
        search.GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int position = ground.WorldToCell(transform.position);
        Vector3Int? updatedPosition =  search.GetNext(position);
        if (updatedPosition != null) {
            transform.position = ground.CellToWorld(updatedPosition.Value);
            }
    }
}
