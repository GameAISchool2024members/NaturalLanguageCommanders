using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BFSearch : MonoBehaviour
{
    [Header("Map information")]
    [SerializeField] Tilemap ground;
    [SerializeField] Sprite walkable;

    [Header("Search object")]
    [SerializeField] GameObject goal;

    private Dictionary<Vector3Int, Vector3Int?> map;

    // Start is called before the first frame update
    void Start()
    {
        // Checks
        if (ground == null || goal == null)
        {
            Debug.Log("Remember to assign variables.");
            enabled = false;
        }

        if (ground.GetSprite(ground.WorldToCell(goal.transform.position)) != walkable)
        {
            Debug.Log("The end position should be on top of a walkable road");
            enabled = false;
        }

        generateMap();
    }

    private void generateMap()
    {
        map = new Dictionary<Vector3Int, Vector3Int?>();
        var frontier = new Queue<Vector3Int>();
        var startCel = ground.WorldToCell(goal.transform.position);
        frontier.Enqueue(startCel);
        map.Add(startCel, null); // goal reached

        while (frontier.Any())
        {
            var current = frontier.Dequeue();
            foreach (var neighbour in getNeighbours(current))
            {
                if (!map.ContainsKey(neighbour))
                {
                    frontier.Enqueue(neighbour);
                    map.Add(neighbour, current);
                }
            }
        }
    }

    private IEnumerable<Vector3Int> getNeighbours(Vector3Int current)
    {
        if (current.x > ground.cellBounds.xMin && ground.GetSprite(current - new Vector3Int(1, 0, 0)) == walkable)
            yield return current - new Vector3Int(1, 0, 0);
        if (current.x < ground.cellBounds.xMax && ground.GetSprite(current + new Vector3Int(1, 0, 0)) == walkable)
            yield return current + new Vector3Int(1, 0, 0);
        if (current.y > ground.cellBounds.yMin && ground.GetSprite(current - new Vector3Int(0, 1, 0)) == walkable)
            yield return current - new Vector3Int(0, 1, 0);
        if (current.y < ground.cellBounds.yMax && ground.GetSprite(current + new Vector3Int(0, 1, 0)) == walkable)
            yield return current + new Vector3Int(0, 1, 0);
    }

    public Vector3Int? GetNext(Vector3Int current) => map.ContainsKey(current) ? map[current] : null;
}
