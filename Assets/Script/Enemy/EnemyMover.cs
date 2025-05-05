using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Pathfinding pathfinding;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Grid _grid;

    private float currentMoveSpeed;
    private Coroutine moveCoroutine;

    // Đảm bảo Start được khởi tạo từ Enemy Data
    private void Start()
    {
        // Nếu bạn có một Enemy component chứa dữ liệu moveSpeed
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            currentMoveSpeed = enemy.enemyData.moveSpeed; // Lấy moveSpeed từ Enemy
        }
        else
        {
            Debug.LogError("Không tìm thấy Enemy component!");
        }
    }

    // Phương thức gán Grid và Target từ bên ngoài
    public void SetTargetAndGrid(Grid newGrid, Transform newTarget)
    {
        _grid = newGrid;
        _targetTransform = newTarget;
    }

    // Trả về tốc độ di chuyển của enemy
    public float GetMoveSpeed()
    {
        return currentMoveSpeed;
    }

    // Gán lại tốc độ di chuyển cho enemy
    public void SetMoveSpeed(float newSpeed)
    {
        currentMoveSpeed = newSpeed;
    }

    // Phương thức để bắt đầu tìm kiếm đường
    public void StartGame()
    {
        RecalculatePath();
    }

    // Phương thức tính toán lại đường đi
    public void RecalculatePath()
    {
        if (_grid == null || _targetTransform == null)
        {
            Debug.LogError("EnemyMover chưa được gán Grid hoặc TargetTransform!");
            return;
        }

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        // Chuyển đổi từ vị trí world sang cell
        Vector3Int startCell = _grid.WorldToCell(transform.position);
        Vector3Int targetCell = _grid.WorldToCell(_targetTransform.position);

        Vector2Int startPos = new Vector2Int(startCell.x, startCell.y);
        Vector2Int targetPos = new Vector2Int(targetCell.x, targetCell.y);

        List<GridNode> path = pathfinding.FindPath(startPos, targetPos);
        if (path != null && path.Count > 0)
        {
            moveCoroutine = StartCoroutine(FollowPath(path)); // Di chuyển theo path đã tính
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đường đi!");
        }
    }

    // Coroutine để di chuyển enemy theo path
    private IEnumerator FollowPath(List<GridNode> path)
    {
        foreach (var node in path)
        {
            Vector3Int cellPos = new Vector3Int(node.x, node.y, 0);
            Vector3 target = _grid.CellToWorld(cellPos) + _grid.cellSize / 2f; // Đảm bảo vị trí đúng tại giữa ô

            // Di chuyển từ vị trí hiện tại tới target
            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, currentMoveSpeed * Time.deltaTime);
                yield return null; // Chờ đến frame sau để tiếp tục di chuyển
            }
        }
    }

    // Đăng ký và hủy đăng ký các sự kiện cập nhật grid
    private void OnEnable()
    {
        GridManager.OnGridUpdated += HandleGridUpdate;
    }

    private void OnDisable()
    {
        GridManager.OnGridUpdated -= HandleGridUpdate;
    }

    // Cập nhật lại đường đi khi grid thay đổi
    private void HandleGridUpdate()
    {
        RecalculatePath();
    }
}