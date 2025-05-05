using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragAndDropPlaceToBlock : DragAndDrop
{
    public Tilemap wallTilemap;

    protected override void Awake()
    {
        base.Awake();
        towerCost = 200;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (CanPick == null || CantPick == null)
            return;

        Vector3Int tilePosition = CanPick.WorldToCell(this.PosMouse());
        TileBase centerTile = CanPick.GetTile(tilePosition);

        if (centerTile == null)
        {
            Debug.Log("Không được đặt tháp ngoài vùng CanPick!");
            objImage.gameObject.SetActive(false);
            CantPick.color = new Color(1, 1, 1, 1f);
            return;
        }

        GridManager gridManager = FindObjectOfType<GridManager>();
        Vector3 cellCenterPosition = CanPick.GetCellCenterWorld(tilePosition);
        Vector3 posObj = cellCenterPosition;
        posObj.z = -3;

        if (!listPosTilemap.Contains(posObj))
        {
            if (!GoldManager.Instance.SpendGold(towerCost))
            {
                Debug.Log("Không đủ vàng để đặt tháp tường!");
                objImage.gameObject.SetActive(false);
                CantPick.color = new Color(1, 1, 1, 1f);
                return;
            }

            TileBase tileSample = null;

            // Lặp qua vùng 1x5 (ngang 1, dọc 5, lấy ô trung tâm làm giữa)
            for (int dy = -2; dy <= 2; dy++)
            {
                Vector3Int currentTilePos = tilePosition + new Vector3Int(0, dy, 0);
                TileBase t = CanPick.GetTile(currentTilePos);

                if (t != null)
                {
                    if (tileSample == null)
                        tileSample = t;

                    // Chuyển tile sang CantPick + wallTilemap
                    CantPick.SetTile(currentTilePos, t);
                    CanPick.SetTile(currentTilePos, null);
                    wallTilemap.SetTile(currentTilePos, t);

                    // ✅ Cập nhật từng ô vào walkableMap
                    if (gridManager != null)
                    {
                        gridManager.UpdateWalkableTileAt(currentTilePos);

                        Vector2Int pos = new Vector2Int(currentTilePos.x, currentTilePos.y);
                        Debug.Log($"Ô {pos} đã cập nhật: đi được? {gridManager.IsWalkable(pos)}");
                    }
                }
            }

            // Đặt tháp chính giữa
            GameObject towerObj = Instantiate(towerPrefab, posObj, Quaternion.identity);
            listPosTilemap.Add(posObj);

            // Gọi hàm Initialize từ TowerWall nếu có
            TowerWall towerScript = towerObj.GetComponent<TowerWall>();
            if (towerScript != null && tileSample != null)
            {
                towerScript.Initialize(tilePosition, CanPick, CantPick, wallTilemap, tileSample);
            }

            // ✅ Gửi sự kiện cập nhật để Enemy biết và tìm lại đường
            gridManager.NotifyGridUpdated();

        }
        else
        {
            Debug.Log("Đã có tháp ở vị trí này hoặc không thể đặt.");
        }

        objImage.gameObject.SetActive(false);
        CantPick.color = new Color(1, 1, 1, 1f);
    }
}
