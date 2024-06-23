using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    public AStarSearch search;
    public Tilemap ground;
    public Tilemap highlight;
    public TileBase highlightColor;
    public AgentVision AgentVision;

    public Vector3? Destination;
    public Vector3 Origin;
    public float Speed = 1.0f;
    public float t = 0;
    public Vector3 cellCorrection = new (0.5f, 0.5f, 0);

    private void Start()
    {
        search.GenerateMap();
        Origin = ground.WorldToCell(transform.position - cellCorrection);
        UpdateTarget();
    }

    private void Update()
    {
        if (Destination != null)
        {
            t += Time.deltaTime;
            var a = t / Speed;
            var newPos = Vector3.Lerp(Origin, Destination.Value, a);
            transform.position = newPos + cellCorrection;

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
        var pos = ground.WorldToCell(transform.position);
        Origin = ground.CellToWorld(pos);

        var newPos = search.GetNext(pos);
        if (newPos != null)
        {
            Destination = ground.CellToWorld(newPos.Value);
            if (AgentVision != null)
            {
                AgentVision.transform.right = (Destination.Value - Origin).normalized;
            }
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
