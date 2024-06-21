using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    public AStarSearch search;
    public Tilemap ground;
    public Tilemap highlight;

    public TileBase highlightColor;

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

    public void DrawMap(){
        highlight.ClearAllTiles();
        foreach(var k in search.mapKeys){
            highlight.SetTile(k, highlightColor);
        }
    }
}
