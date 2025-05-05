using UnityEngine;
using TMPro;

public class CheckLossManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text livesText;  // Số mạng còn
    [SerializeField] private GameObject lossUI;   // UI hiển thị khi thua

    private int totalLives = 3;  // Mạng bắt đầu
    private int currentLives;

    private void Start()
    {
        currentLives = totalLives;
        UpdateLivesUI();
        lossUI.SetActive(false);  // Ẩn UI thua khi bắt đầu
    }

    // Gọi từ collider khi Enemy đi qua
    public void DecreaseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            ShowLossUI();
            StopGame();
        }
    }

    // Cập nhật số mạng trong UI
    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "" + currentLives;
        }
    }

    // Hiển thị UI thua
    private void ShowLossUI()
    {
        lossUI.SetActive(true);
        Debug.Log("You Lose!");
    }

    // Dừng trò chơi
    private void StopGame()
    {
        Time.timeScale = 0;  // Dừng trò chơi
        Debug.Log("Game Over! Game Paused.");
    }
}
