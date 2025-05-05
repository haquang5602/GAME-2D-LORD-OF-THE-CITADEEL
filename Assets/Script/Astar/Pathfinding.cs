using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public int x, y;
    public int gCost, hCost;
    public int fCost => gCost + hCost;
    public GridNode parent;

    public GridNode(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2Int Pos => new Vector2Int(x, y);
}

public class Pathfinding : MonoBehaviour
{
    public GridManager gridManager;

    public List<GridNode> FindPath(Vector2Int startWorldPos, Vector2Int endWorldPos)
    {
        Vector2Int startPos = startWorldPos - gridManager.gridOrigin;
        Vector2Int endPos = endWorldPos - gridManager.gridOrigin;

        int width = gridManager.width;
        int height = gridManager.height;

        // Kiểm tra hợp lệ
        if (!IsInBounds(startPos, width, height) || !IsInBounds(endPos, width, height))
        {
            Debug.LogWarning("Start hoặc End nằm ngoài lưới!");
            return null;
        }

        GridNode[,] nodes = new GridNode[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                nodes[x, y] = new GridNode(x, y);

        GridNode startNode = nodes[startPos.x, startPos.y];
        GridNode endNode = nodes[endPos.x, endPos.y];

        List<GridNode> openSet = new List<GridNode> { startNode };
        HashSet<GridNode> closedSet = new HashSet<GridNode>();

        while (openSet.Count > 0)
        {
            GridNode current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost ||
                    (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == endNode)
                return RetracePath(startNode, endNode);

            foreach (Vector2Int dir in Directions)
            {
                int nx = current.x + dir.x;
                int ny = current.y + dir.y;

                if (!IsInBounds(new Vector2Int(nx, ny), width, height))
                    continue;

                if (!gridManager.walkableMap[nx, ny])
                    continue;

                GridNode neighbor = nodes[nx, ny];
                if (closedSet.Contains(neighbor))
                    continue;

                int newCost = current.gCost + 1;
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = Heuristic(neighbor.Pos, endPos);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    List<GridNode> RetracePath(GridNode start, GridNode end)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode current = end;
        while (current != start)
        {
            path.Add(new GridNode(current.x + gridManager.gridOrigin.x, current.y + gridManager.gridOrigin.y)); // Chuyển về toạ độ thế giới
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan
    }

    bool IsInBounds(Vector2Int pos, int width, int height)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    static readonly Vector2Int[] Directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };
}
