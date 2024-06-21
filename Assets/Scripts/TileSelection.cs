using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelection : MonoBehaviour
{

    [Header("Map information")]

    [SerializeField] public Tilemap tilemap;

    [SerializeField] public GameObject goal;

    private Vector3Int highlightedTilePosition = Vector3Int.zero;

    public Vector3Int GetHighlightedTile()
    {
        return highlightedTilePosition;
    }

    public bool IsHighlightedTileClicked(Vector3 clickedTile)
    {
        Vector3Int gridPosition = tilemap.WorldToCell(clickedTile);
        return gridPosition == highlightedTilePosition;
    }
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        highlightedTilePosition = tilemap.WorldToCell(mouseWorldPosition);

    }
}
