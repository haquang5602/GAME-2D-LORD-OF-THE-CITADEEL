using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MapWaveData
{
    public string mapName;
    public MapWaveConfigSO waveConfig;
}

public class WaveManager : Spawns
{
    public static WaveManager instance;

    [Header("Enemy Settings")]
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Quaternion spawnRotation = Quaternion.identity;

    [Header("Map Data")]
    [SerializeField] private List<MapWaveData> mapWaveConfigs;
    private MapWaveConfigSO waveConfig;

    [Header("Wave Settings")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int enemiesPerType = 1;
    [SerializeField] private float spawnDelay = 0.1f;

    [Header("UI")]
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject winPanel;

    private int aliveEnemies = 0;

    protected override void Start()
    {
        base.Start();
        instance = this;

        string selectedMapName = PlayerPrefs.GetString("Map", "");
        if (string.IsNullOrEmpty(selectedMapName))
        {
            Debug.LogError("Không có tên map trong PlayerPrefs!");
            return;
        }

        foreach (var mapData in mapWaveConfigs)
        {
            if (mapData.mapName == selectedMapName)
            {
                waveConfig = mapData.waveConfig;
                break;
            }
        }

        if (waveConfig == null)
        {
            Debug.LogError("Không tìm thấy MapWaveConfigSO phù hợp với tên map: " + selectedMapName);
            return;
        }

        spawnPosition = waveConfig.spawnPosition;

        if (this.prefab == null)
            this.prefab = transform.Find("Prefab");

        if (this.prefabs.Count == 0 && this.prefab != null)
        {
            foreach (Transform t in this.prefab)
            {
                this.prefabs.Add(t);
            }
        }

        if (this.holder == null)
            this.holder = transform.Find("Holder");

        playButton.onClick.AddListener(() =>
        {
            playButton.gameObject.SetActive(false);
            StartCoroutine(SpawnWaveCoroutine());
        });

        UpdateWaveUI();
    }

    IEnumerator SpawnWaveCoroutine()
    {
        while (currentWave < waveConfig.waveLists.Count)
        {
            List<int> wave = waveConfig.waveLists[currentWave].enemies;

            foreach (int enemyType in wave)
            {
                for (int i = 0; i < enemiesPerType; i++)
                {
                    string nameEnemy = GetNameEnemy(enemyType);

                    Transform enemy = Spawn(nameEnemy, spawnPosition, spawnRotation);
                    if (enemy == null)
                    {
                        Debug.LogError($"Spawn lỗi: không tạo được enemy {nameEnemy}");
                        continue;
                    }

                    aliveEnemies++; // ✅ Tăng số enemy đang sống

                    EnemyMover mover = enemy.GetComponent<EnemyMover>();
                    if (mover != null)
                    {
                        Grid currentGrid = MapLoader.Instance.CurrentGrid;
                        Transform homeTransform = MapLoader.Instance.CurrentHome;
                        mover.SetTargetAndGrid(currentGrid, homeTransform);
                        mover.StartGame();
                    }

                    yield return new WaitForSeconds(spawnDelay);
                }
            }
            
            currentWave++;
            UpdateWaveUI();

            yield return new WaitForSeconds(10f);
        }

        // ❗Không hiện Win UI tại đây vì vẫn còn quái → chờ tất cả bị tiêu diệt
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave}/{waveConfig.waveLists.Count}";
        }
    }

    private string GetNameEnemy(int type)
    {
        switch (type)
        {
            case 1: return "Small";
            case 2: return "Giant";
            case 3: return "Wizard";
            case 4: return "Witch";
            default: return "Small";
        }
    }

    // Gọi từ Enemy.cs khi enemy chết
    public void OnEnemyKilled()
    {
        aliveEnemies--;
        Debug.Log(aliveEnemies);
        if (currentWave >= waveConfig.waveLists.Count && aliveEnemies <= 0)
        {
            ShowWinUI();
        }
    }

    private void ShowWinUI()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
}
