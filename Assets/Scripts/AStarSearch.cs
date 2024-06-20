using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

public class AStarSearch : MonoBehaviour
{
    [Header("Map information")]
    [SerializeField] Tilemap tilemap;
    [SerializeField] Sprite walkableSprite = null;

    [Header("Search object")]
    [SerializeField] GameObject goal;
    [SerializeField] GameObject agent;

    Vector3Int goalPos;
    Vector3Int agentPos;

    private Dictionary<Vector3Int, Vector3Int?> map;

    // Start is called before the first frame update
    private void OnEnable()
    { 
        if (tilemap != null)
        {
            Debug.Log("You haven't attached a tilemap, so if you're not gonna need me I'm just gonna turn myself off.");
            enabled = false;
        }
    }

    private int GetHeuristic(Vector3Int position)
    {
        // Manhatten distance
        return  Mathf.Abs(position.x - goalPos.x) +
                Mathf.Abs(position.y - goalPos.y) +
                Mathf.Abs(position.z - goalPos.z);
    }

    private Dictionary<Vector3Int, (Vector3Int? position, int price)> Search()
    {
        if (tilemap.GetSprite(goalPos) != walkableSprite)
            goalPos = getNeighbours(goalPos).First(); // Make sure the goal is walkable

        var blackboard = new Dictionary<Vector3Int, (Vector3Int? position, int price)>();
        var frontier = new PriorityQueue<Vector3Int, int>();
        frontier.Enqueue(agentPos, 0);
        blackboard.Add(agentPos, (null, 0));

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            foreach (var neighbour in getNeighbours(current))
            {
                var price = blackboard[current].price + 1;
                if (!blackboard.ContainsKey(neighbour) || blackboard[neighbour].price > price)
                {
                    frontier.Enqueue(neighbour, price + GetHeuristic(neighbour));
                    blackboard.Add(neighbour, (current, price));
                    if (neighbour == goalPos)
                        return blackboard;
                }
            }
        }
        return new Dictionary<Vector3Int, (Vector3Int? positin, int price)> { { goalPos, (null, 0) } };
    }

    public void GenerateMap()
    {
        goalPos = tilemap.WorldToCell(goal.transform.position);
        agentPos = tilemap.WorldToCell(agent.transform.position);

        map = new Dictionary<Vector3Int, Vector3Int?>();
        var searchResult = Search();
        Vector3Int? previous = null;
        Vector3Int? current = goalPos;
        while (current != null)
        {
            map.Add(current.Value, previous);
            previous = current;
            current = searchResult[current.Value].position;
        }
    }

    private IEnumerable<Vector3Int> getNeighbours(Vector3Int current)
    {
        if (current.x > tilemap.cellBounds.xMin && tilemap.GetSprite(current - new Vector3Int(1, 0, 0)) == walkableSprite)
            yield return current - new Vector3Int(1, 0, 0);
        if (current.x < tilemap.cellBounds.xMax && tilemap.GetSprite(current + new Vector3Int(1, 0, 0)) == walkableSprite)
            yield return current + new Vector3Int(1, 0, 0);
        if (current.y > tilemap.cellBounds.yMin && tilemap.GetSprite(current - new Vector3Int(0, 1, 0)) == walkableSprite)
            yield return current - new Vector3Int(0, 1, 0);
        if (current.y < tilemap.cellBounds.yMax && tilemap.GetSprite(current + new Vector3Int(0, 1, 0)) == walkableSprite)
            yield return current + new Vector3Int(0, 1, 0);
    }

    public Vector3Int? GetNext(Vector3Int current) => map.ContainsKey(current) ? map[current] : null;
}
