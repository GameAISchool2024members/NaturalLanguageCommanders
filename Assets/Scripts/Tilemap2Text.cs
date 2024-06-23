using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemap2Text : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject player;
    [SerializeField] GPTController[] guards;

    private void Start()
    {
        Debug.Log(ConvertTilemap());
    }

    private Dictionary<Vector3Int, int> GetGuardPositions()
    {
        Dictionary<Vector3Int, int> output = new();
        for (int i =0; i < guards.Length; i++)
        {
            var cell = tilemap.WorldToCell(guards[i].transform.position);
            if (!output.ContainsKey(cell))
                output.Add(cell, i);
        }
        return output;
    }

    public string ConvertTilemap()
    {
        var playerPos = tilemap.WorldToCell(player.transform.position);
        var guardPos = GetGuardPositions();
        var map = MapLabelController.Instance.MapLabels();
        var legends = new List<MapLabel>();

        var output = new StringBuilder();
        for (int y = tilemap.cellBounds.yMax-1; y >= tilemap.cellBounds.yMin; y--)
        {
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                var cell = new Vector3Int(x, y, 0);
                if (playerPos == cell)
                {
                    output.Append('+');
                }
                else if (map.ContainsKey(cell))
                {
                    legends.Add(map[cell]);
                    output.Append((char)(64 + legends.Count));
                }
                else if (guardPos.ContainsKey(cell))
                {
                    output.Append(guardPos[cell]+1);
                }
                else if (tilemap.GetSprite(cell) != null)
                {
                    output.Append('#');
                }
                else
                {
                    output.Append('.');
                }
            }
            output.Append("\n");
        }
        output.AppendLine("Here are the map legends:");
        output.AppendLine("#=Obstacle");
        output.AppendLine(".=Empty space");
        output.AppendLine("+=Player (target)");
        for (int i = 0; i < guards.Length; i++)
        {
            output.AppendLine($"{i + 1}={guards[i].name}");
        }

        for (int i = 0; i < legends.Count; i++)
        {
            output.AppendLine($"{(char)(65 + i)}={legends[i].name}");
        }

        var str = output.ToString();
        Debug.Log(str);
        return str;
    }
}
