using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GridMovement gridMovement;

    [SerializeField] TileSelection tileSelection; 


    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(IsHighlightedTileClicked(mouseWorldPosition));
            // Debug.Log("Clicked on tile: " + highlightedTilePosition);
            tileSelection.goal.transform.position = tileSelection.tilemap.GetCellCenterWorld(tileSelection.GetHighlightedTile());
            gridMovement.search.GenerateMap();
            gridMovement.DrawMap();
            // gridMovement.FollowGoal();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GridMovement gridMovement;

    [SerializeField] TileSelection tileSelection; 


    void Update ()
    {
        gridMovement.DrawMap();
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(IsHighlightedTileClicked(mouseWorldPosition));
            // Debug.Log("Clicked on tile: " + highlightedTilePosition);
            tileSelection.goal.transform.position = tileSelection.tilemap.GetCellCenterWorld(tileSelection.GetHighlightedTile());
            gridMovement.search.GenerateMap();
            // gridMovement.FollowGoal();
        }
    }
}
