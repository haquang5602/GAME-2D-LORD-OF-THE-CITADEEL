using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
