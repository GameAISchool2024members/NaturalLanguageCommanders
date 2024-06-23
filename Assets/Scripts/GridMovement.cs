using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    public AStarSearch search;
    public Tilemap ground;
    public Tilemap highlight;
    public TileBase highlightColor;

    public Vector3? Destination;
    public Vector3 Origin;
    public float Speed = 1.0f;
    public float t = 0;

    private void Start()
    {
        search.GenerateMap();
        Origin = ground.WorldToCell(transform.position);
        Destination = null;
    }

    private void Update()
    {
        if (Destination != null)
        {
            t += Time.deltaTime;
            var a = t / Speed;
            transform.position = Vector3.Lerp(Origin, Destination.Value, a);
            if (a >= 1)
            {
                UpdateTarget();
            }
        }
    }

    public void UpdateTarget()
    {
        search.GenerateMap();
        DrawMap();
        Origin = transform.position;
        var pos = ground.WorldToCell(transform.position);
        var newPos = search.GetNext(pos);
        if (newPos != null)
        {
            Destination = ground.CellToWorld(newPos.Value);
        }
        else
        {
            Destination = null;
        }
        t = 0;
    }

    public void DrawMap(){
        highlight.ClearAllTiles();
        foreach(var k in search.mapKeys){
            highlight.SetTile(k, highlightColor);
        }
    }
}
