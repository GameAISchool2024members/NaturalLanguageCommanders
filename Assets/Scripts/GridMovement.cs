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
    public void FollowGoal()
    {
        Vector3Int position = ground.WorldToCell(transform.position);
        Vector3Int? updatedPosition =  search.GetNext(position);
        if (updatedPosition != null) {
            transform.position = ground.GetCellCenterWorld(updatedPosition.Value);
            }
    }
}
