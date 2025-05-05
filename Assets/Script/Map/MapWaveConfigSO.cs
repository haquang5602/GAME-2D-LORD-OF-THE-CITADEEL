using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [Tooltip("Danh sách loại enemy trong wave này")]
    public List<int> enemies;  // Danh sách các loại enemy trong wave

    public Wave(List<int> enemies)
    {
        this.enemies = enemies;
    }
}

[CreateAssetMenu(fileName = "MapWaveConfig", menuName = "ScriptableObjects/MapWaveConfig", order = 1)]
public class MapWaveConfigSO : ScriptableObject
{
    public string mapName;
    public Vector3 spawnPosition;

    [Tooltip("Danh sách các wave trong map")]
    public List<Wave> waveLists;  // Thay List<List<int>> bằng List<Wave>
}
