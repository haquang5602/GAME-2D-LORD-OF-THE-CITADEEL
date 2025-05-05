using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CheckLossManager checkLossManager = FindObjectOfType<CheckLossManager>();
            if (checkLossManager != null)
            {
                checkLossManager.DecreaseLife();
            }

            if (WaveManager.instance != null)
            {
                WaveManager.instance.OnEnemyKilled();
            }

            Destroy(other.gameObject);
        }
    }
}
