using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager instance; 

    public Tilemap wallTilemap;

    public bool[,] walkableMap;
    public Vector2Int gridOrigin;
    public int width;
    public int height;

    public static event Action OnGridUpdated;

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Có nhiều hơn 1 GridManager tồn tại!");
            Destroy(gameObject);
            return;
        }
        // Nếu wallTilemap được gán từ ngoài (như MapLoader), mới khởi tạo
        if (wallTilemap != null)
        {
            InitializeWalkableMap();
        }

        BoundsInt bounds = wallTilemap.cellBounds;
        gridOrigin = new Vector2Int(bounds.xMin, bounds.yMin);
        width = bounds.size.x;
        height = bounds.size.y;

        walkableMap = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cell = new Vector3Int(x + gridOrigin.x, y + gridOrigin.y, 0);
                bool hasWall = wallTilemap.HasTile(cell);
                walkableMap[x, y] = !hasWall;
            }
        }

        Debug.Log($"GridManager khởi tạo xong. Kích thước: {width}x{height}, Origin: {gridOrigin}");
    }

    public void NotifyGridUpdated()
    {
        OnGridUpdated?.Invoke();
    }

    public bool IsWalkable(Vector2Int pos)
    {
        Vector2Int local = pos - gridOrigin;
        if (local.x < 0 || local.x >= width || local.y < 0 || local.y >= height)
            return false;
        return walkableMap[local.x, local.y];
    }

    public void UpdateWalkableTileAt(Vector3Int cell)
    {
        Vector2Int local = new Vector2Int(cell.x, cell.y) - gridOrigin;

        if (local.x >= 0 && local.x < width && local.y >= 0 && local.y < height)
        {
            walkableMap[local.x, local.y] = false;
            Debug.Log($"Cập nhật ô {cell}: KHÔNG đi được");
        }
    }

    public void UpdateWalkableAreaAround(Vector3Int center)
    {
        for (int dy = -2; dy <= 2; dy++)
        {
            Vector3Int cell = new Vector3Int(center.x, center.y + dy, 0);
            if (wallTilemap.HasTile(cell))
            {
                UpdateWalkableTileAt(cell);
            }
        }

        OnGridUpdated?.Invoke();
    }

    public void SetWalkable(Vector3Int cell, bool isWalkable)
    {
        Vector2Int local = new Vector2Int(cell.x, cell.y) - gridOrigin;
        if (local.x >= 0 && local.x < width && local.y >= 0 && local.y < height)
        {
            walkableMap[local.x, local.y] = isWalkable;
            Debug.Log($"Cập nhật lại ô {cell}: {(isWalkable ? "đi được" : "KHÔNG đi được")}");
        }
    }

    public void UpdateWalkableAreaAfter(Vector3Int center)
    {
        for (int dy = -2; dy <= 2; dy++)
        {
            Vector3Int cell = new Vector3Int(center.x, center.y + dy, 0);
            Vector2Int local = new Vector2Int(cell.x, cell.y) - gridOrigin;

            if (local.x >= 0 && local.x < width && local.y >= 0 && local.y < height)
            {
                if (!wallTilemap.HasTile(cell))
                {
                    walkableMap[local.x, local.y] = true;
                    Debug.Log($"Ô {cell}: cập nhật lại là có thể đi được.");
                }
            }
        }
    }
    public void InitializeWalkableMap()
    {
        if (wallTilemap == null)
        {
            Debug.LogError("wallTilemap chưa được gán!");
            return;
        }

        BoundsInt bounds = wallTilemap.cellBounds;
        gridOrigin = new Vector2Int(bounds.xMin, bounds.yMin);
        width = bounds.size.x;
        height = bounds.size.y;

        walkableMap = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cell = new Vector3Int(x + gridOrigin.x, y + gridOrigin.y, 0);
                bool hasWall = wallTilemap.HasTile(cell);
                walkableMap[x, y] = !hasWall;
            }
        }

        //Debug.Log($"Đã khởi tạo lại walkableMap từ wallTilemap: {width}x{height}, Origin: {gridOrigin}");
        NotifyGridUpdated();
    }

}
