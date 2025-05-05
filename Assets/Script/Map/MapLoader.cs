using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    [System.Serializable]
    public class MapEntry
    {
        public string mapName;
        public GameObject prefab;
    }

    public List<MapEntry> mapPrefabs;

    public DragFire fireDragAndDrop;
    public DragMagic magicDragAndDrop;
    public DragFlash flashDragAndDrop;
    public DragAndDropPlaceToBlock wallDragAndDrop;

    public static MapLoader Instance { get; private set; }
    public Grid CurrentGrid { get; private set; }
    public Transform CurrentHome { get; private set; }
    public GameObject CurrentEndzone { get; private set; }  // Đã đổi sang GameObject

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        string selectedMapName = PlayerPrefs.GetString("Map", "");

        if (string.IsNullOrEmpty(selectedMapName))
        {
            Debug.LogError("Không có tên map được lưu trong PlayerPrefs!");
            return;
        }

        GameObject selectedPrefab = null;
        foreach (var entry in mapPrefabs)
        {
            if (entry.mapName == selectedMapName)
            {
                selectedPrefab = entry.prefab;
                break;
            }
        }

        if (selectedPrefab == null)
        {
            Debug.LogError("Không tìm thấy prefab map có tên: " + selectedMapName);
            return;
        }

        GameObject mapInstance = Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);

        Tilemap canPick = mapInstance.transform.Find("CanPick")?.GetComponent<Tilemap>();
        Tilemap cantPick = mapInstance.transform.Find("CanNotPick")?.GetComponent<Tilemap>();
        Tilemap canPickWall = mapInstance.transform.Find("CanPickWall")?.GetComponent<Tilemap>();
        Tilemap canPickBomp = mapInstance.transform.Find("CanPickBomp")?.GetComponent<Tilemap>();
        Transform homeTransform = mapInstance.transform.Find("Home");
        GameObject endzoneObject = mapInstance.transform.Find("Endzone")?.gameObject;
        Grid grid = mapInstance.GetComponent<Grid>();

        // Lưu lại Grid, Home và Endzone (game object)
        CurrentGrid = grid;
        CurrentHome = homeTransform;
        CurrentEndzone = endzoneObject;

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        BombPlacer bomp = FindObjectOfType<BombPlacer>();

        if (bomp != null)
        {
            bomp.SetGroundTilemap(canPickBomp);
        }

        if (player != null)
        {
            player.SetCanNotPickTilemap(cantPick);
        }

        if (cantPick != null)
        {
            GridManager.instance.wallTilemap = cantPick;
        }

        if (canPick != null && fireDragAndDrop != null)
        {
            fireDragAndDrop.CanPick = canPick;
            fireDragAndDrop.CantPick = cantPick;
        }

        if (canPick != null && magicDragAndDrop != null)
        {
            magicDragAndDrop.CanPick = canPick;
            magicDragAndDrop.CantPick = cantPick;
        }

        if (canPick != null && flashDragAndDrop != null)
        {
            flashDragAndDrop.CanPick = canPick;
            flashDragAndDrop.CantPick = cantPick;
        }

        if (canPickWall != null && wallDragAndDrop != null)
        {
            wallDragAndDrop.CanPick = canPickWall;
            wallDragAndDrop.CantPick = cantPick;
            wallDragAndDrop.wallTilemap = cantPick;
        }

        Debug.Log("Đã load map: " + selectedMapName);

        if (GridManager.instance != null)
        {
            GridManager.instance.InitializeWalkableMap();
        }
    }
}
