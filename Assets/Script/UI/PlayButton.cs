using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayButton : MonoBehaviour
{
    public void OnPlayClicked()
    {
        string selectedMapName = ScrollLevel.nameMap;
        PlayerPrefs.SetString("Map", selectedMapName);

        Debug.Log("Map name: " + selectedMapName);

        SceneManager.LoadScene("Game");
        // TODO: Load map theo tên, lưu vào GameManager, v.v.
    }
}

