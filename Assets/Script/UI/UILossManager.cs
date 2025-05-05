using UnityEngine;
using UnityEngine.SceneManagement;  // Để sử dụng SceneManager

public class UILossManager : MonoBehaviour
{
    
    public void OnReplayButtonClicked()
    {
       
        //string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }

    
    public void OnMenuButtonClicked()
    {
        
        SceneManager.LoadScene("Menu"); 
        Time.timeScale = 1;
    }
}
