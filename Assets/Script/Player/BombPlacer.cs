using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class BombPlacer : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private float bombYOffset = -0.1f;
    [SerializeField] private int maxBombCount = 5;

    [Header("UI References")]
    [SerializeField] private TMP_Text textE;
    [SerializeField] private Image panelE;


    private int currentBombCount;
    private Color originalPanelColor; // Màu ban đầu của panel

    private void Start()
    {
        currentBombCount = maxBombCount;

        if (panelE != null)
        {
            originalPanelColor = panelE.color; // Lưu màu ban đầu
        }

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlaceBomb();
        }
    }

    private void TryPlaceBomb()
    {
        if (bombPrefab == null || groundTilemap == null)
        {
            Debug.LogWarning("Thiếu prefab bomb hoặc Tilemap!");
            return;
        }

        if (currentBombCount <= 0)
        {
            Debug.Log("Hết bom rồi!");
            return;
        }

        Vector3Int cellPosition = groundTilemap.WorldToCell(transform.position);
        Vector3 placePosition = groundTilemap.GetCellCenterWorld(cellPosition);
        placePosition.y += bombYOffset;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(placePosition, 0.1f);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Bomb"))
            {
                Debug.Log("Đã có bom tại vị trí này");
                return;
            }
        }

        Instantiate(bombPrefab, placePosition, Quaternion.identity);
        currentBombCount--;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (textE != null)
        {
            textE.text = currentBombCount.ToString();
        }

        if (panelE != null)
        {
            if (currentBombCount <= 0)
            {
                panelE.color = Color.red; // Hết bom -> đổi panel thành đỏ
            }
            else
            {
                panelE.color = originalPanelColor; // Còn bom -> giữ nguyên màu ban đầu
            }
        }
    }
    public void SetGroundTilemap(Tilemap tilemap)
    {
        groundTilemap = tilemap;
    }

}
