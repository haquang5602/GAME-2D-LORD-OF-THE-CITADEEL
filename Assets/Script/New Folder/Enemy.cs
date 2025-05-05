using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;

    private float currentHealth;
    private EnemyHealthBar healthBar;

    void Start()
    {
        currentHealth = enemyData.maxHealth;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, enemyData.maxHealth);
        }
    }

    void Die()
    {
        GoldManager.Instance.AddGold(enemyData.goldReward);

        
        if (WaveManager.instance != null)
        {
            WaveManager.instance.OnEnemyKilled();
        }

        Destroy(gameObject);
    }
}
