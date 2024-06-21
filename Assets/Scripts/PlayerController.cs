using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GridMovement gridMovement;

    [SerializeField] TileSelection tileSelection; 


    void Update ()
    {
<<<<<<< Updated upstream
        gridMovement.DrawMap();
=======
        
>>>>>>> Stashed changes
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(IsHighlightedTileClicked(mouseWorldPosition));
            // Debug.Log("Clicked on tile: " + highlightedTilePosition);
            tileSelection.goal.transform.position = tileSelection.tilemap.GetCellCenterWorld(tileSelection.GetHighlightedTile());
            gridMovement.search.GenerateMap();
<<<<<<< Updated upstream
=======
            gridMovement.DrawMap();
>>>>>>> Stashed changes
            // gridMovement.FollowGoal();
        }
    }
}
