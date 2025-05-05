using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerWall : MonoBehaviour
{
    private Vector3Int centerCell;
    private Tilemap canPick, cantPick, wallTilemap;

    private Dictionary<Vector3Int, TileBase> originalTiles = new Dictionary<Vector3Int, TileBase>();

    [SerializeField] private float destroyDelay = 5f;

    public void Initialize(Vector3Int cell, Tilemap canPickMap, Tilemap cantPickMap, Tilemap wallMap, TileBase fallbackTile)
    {
        centerCell = cell;
        canPick = canPickMap;
        cantPick = cantPickMap;
        wallTilemap = wallMap;

        // 🔄 Lưu lại tile gốc cho vùng 1x5 (nằm ngang)
        for (int dx = -2; dx <= 2; dx++)
        {
            Vector3Int pos = centerCell + new Vector3Int(dx, 0, 0);
            TileBase original = canPick.GetTile(pos);
            originalTiles[pos] = original != null ? original : fallbackTile;
        }

        ProgressBarTimer progress = GetComponentInChildren<ProgressBarTimer>();
        if (progress != null)
        {
            progress.SetDuration(destroyDelay);
            progress.StartProgress();
        }

        StartCoroutine(RemoveAfterSeconds(destroyDelay));
    }

    private IEnumerator RemoveAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // 🔄 Xóa vùng 1x5
        for (int dx = -2; dx <= 2; dx++)
        {
            Vector3Int pos = centerCell + new Vector3Int(dx, 0, 0);

            if (cantPick.HasTile(pos))
            {
                wallTilemap.SetTile(pos, null);
                cantPick.SetTile(pos, null);

                if (originalTiles.TryGetValue(pos, out TileBase originalTile))
                {
                    canPick.SetTile(pos, originalTile);
                }
            }
        }

        GridManager gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            gridManager.UpdateWalkableAreaAfter(centerCell);
        }

        EnemyMover[] enemies = FindObjectsOfType<EnemyMover>();
        foreach (var enemy in enemies)
        {
            enemy.RecalculatePath();
        }

        Destroy(gameObject);
    }
}
