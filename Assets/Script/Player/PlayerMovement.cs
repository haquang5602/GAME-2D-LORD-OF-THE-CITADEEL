using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Tilemap canNotPickTilemap;

    private Vector2 movement;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (canNotPickTilemap == null)
        {
            Debug.LogWarning("CanNotPick Tilemap chưa được gán!");
        }
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            TryMove(movement.normalized);
            FlipSprite(movement.x);
        }
    }

    private void TryMove(Vector2 direction)
    {
        if (canNotPickTilemap == null)
        {
            Debug.LogWarning("CanNotPick Tilemap chưa được gán trong TryMove!");
            return;
        }

        Vector3 newPosition = transform.position + (Vector3)direction * moveSpeed * Time.deltaTime;
        Vector3Int cellPos = canNotPickTilemap.WorldToCell(newPosition);
        TileBase tile = canNotPickTilemap.GetTile(cellPos);

        if (tile == null) // Chỉ di chuyển nếu tile không bị cấm
        {
            transform.position = newPosition;
        }
        else
        {
            Debug.Log("Không thể di chuyển vào vùng cấm!");
        }
    }

    private void FlipSprite(float horizontalInput)
    {
        float scaleX = 0.5f;

        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(-scaleX, scaleX, 1); // Sang phải: xoay mặt phải và thu nhỏ
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(scaleX, scaleX, 1); // Sang trái: xoay mặt trái và thu nhỏ
        }
    }

    public void SetCanNotPickTilemap(Tilemap tilemap)
    {
        this.canNotPickTilemap = tilemap;
    }


}
