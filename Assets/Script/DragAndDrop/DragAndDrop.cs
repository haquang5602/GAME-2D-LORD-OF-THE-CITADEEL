using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragAndDrop : GapiMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static DragAndDrop instance;

    [SerializeField] protected Tilemap _canPick;  
    [SerializeField] protected Tilemap _cantPick; 
    [SerializeField] protected Transform objImage;
    [SerializeField] protected GameObject towerPrefab;

    [SerializeField] protected int towerCost;

    protected static List<Vector3> listPosTilemap = new List<Vector3>();

    protected override void Awake()
    {
        base.Awake();
        DragAndDrop.instance = this;
    }

    // Thêm getter và setter để truy cập từ ngoài
    public Tilemap CanPick
    {
        get { return _canPick; }
        set { _canPick = value; }
    }

    public Tilemap CantPick
    {
        get { return _cantPick; }
        set { _cantPick = value; }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_canPick == null || _cantPick == null || towerPrefab == null)
        {
            Debug.LogError("Tilemap hoặc Prefab tháp chưa được gán trong Inspector!");
            return;
        }

        objImage.gameObject.SetActive(true);
        _cantPick.color = new Color(0.5f, 0.5f, 0.5f, 1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canPick == null) return;

        Vector3 posObj = this.PosMouse();
        posObj.z = -3;
        objImage.transform.position = posObj;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (_canPick == null || _cantPick == null)
        {
            Debug.LogError("Tilemap chưa được gán trong Inspector!");
            return;
        }

        Vector3Int tilePosition = _canPick.WorldToCell(this.PosMouse()); // Xác định ô trong tilemap
        Vector3 cellCenterPosition = _canPick.GetCellCenterWorld(tilePosition); // lấy vị trí chính giữa ô
        Vector3 posObj = cellCenterPosition;
        posObj.z = -3;

        if (_canPick.GetTile(tilePosition) != null && !listPosTilemap.Contains(posObj))
        {
            if (GoldManager.Instance.SpendGold(towerCost))
            {
                Instantiate(towerPrefab, posObj, Quaternion.identity);
                listPosTilemap.Add(posObj);
            }
            else
            {
                Debug.Log("Không đủ vàng để mua tháp!");
            }
        }
        else
        {
            Debug.Log("Đã có tháp ở vị trí này hoặc không thể đặt.");
        }

        objImage.gameObject.SetActive(false);
        _cantPick.color = new Color(1, 1, 1, 1f);
    }

    protected Vector3 PosMouse()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = -3; // Đặt Z để chắc chắn hiển thị đúng
        return pos;
    }
}
