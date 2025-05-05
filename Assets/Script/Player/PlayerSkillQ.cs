using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerSkillQ : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float lightningDuration = 1f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private float slowDuration = 5f;
    [SerializeField] private float skillRange = 100f;
    [SerializeField] private Vector2 centerPosition = Vector2.zero;

    [Header("Camera Shake Settings")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    [Header("UI References")]
    [SerializeField] private TMP_Text textQ;
    [SerializeField] private TMP_Text cooldownText; // 👉 Text hiển thị thông báo cooldown
    [SerializeField] private Image panelQ;
    [SerializeField] private int maxSkillUses = 2;

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownDuration = 5f; // 👉 Thời gian hồi chiêu giữa mỗi lần dùng

    private int currentSkillUses;
    private bool isCooldown = false;
    private Color originalPanelColor;

    private void Start()
    {
        currentSkillUses = maxSkillUses;

        if (panelQ != null)
        {
            originalPanelColor = panelQ.color;
        }

        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(false); // Ẩn text cooldown lúc đầu
        }

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryActivateLightning();
        }
    }

    private void TryActivateLightning()
    {
        if (currentSkillUses <= 0)
        {
            Debug.Log("Hết lượt dùng Skill Q!");
            return;
        }

        if (isCooldown)
        {
            Debug.Log("Đang hồi chiêu!");
            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
                cooldownText.text = "Đang hồi chiêu!";
                Invoke(nameof(HideCooldownText), 1.5f); // Ẩn text sau 1.5 giây
            }
            return;
        }

        ActivateLightning();
        currentSkillUses--;
        UpdateUI();

        if (currentSkillUses > 0)
        {
            StartCoroutine(StartCooldown()); // Bắt đầu đếm thời gian hồi chiêu
        }
    }

    private void ActivateLightning()
    {
        GameObject lightning = Instantiate(lightningPrefab, centerPosition, Quaternion.identity);
        Destroy(lightning, lightningDuration);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(centerPosition, skillRange, enemyLayer);
        foreach (var enemyCollider in enemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            EnemyMover mover = enemyCollider.GetComponent<EnemyMover>();
            if (mover != null)
            {
                StartCoroutine(ApplySlow(mover));
            }
        }

        if (CameraShake.Instance != null)
        {
            StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeMagnitude));
        }
    }

    IEnumerator ApplySlow(EnemyMover mover)
    {
        float originalSpeed = mover.GetMoveSpeed();
        mover.SetMoveSpeed(originalSpeed * slowFactor);

        SpriteRenderer sprite = mover.GetComponent<SpriteRenderer>();
        Color originalColor = sprite != null ? sprite.color : Color.white;

        if (sprite != null)
        {
            sprite.color = Color.magenta;
        }

        yield return new WaitForSeconds(slowDuration);

        mover.SetMoveSpeed(originalSpeed);

        if (sprite != null)
        {
            sprite.color = originalColor;
        }
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }

    private void HideCooldownText()
    {
        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (textQ != null)
        {
            textQ.text = currentSkillUses.ToString();
        }

        if (panelQ != null)
        {
            if (currentSkillUses <= 0)
            {
                panelQ.color = Color.red;
            }
            else
            {
                panelQ.color = originalPanelColor;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centerPosition, skillRange);
    }
}
