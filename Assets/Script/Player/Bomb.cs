using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 1f;
    [SerializeField] private float explosionRadius = 5.0f;
    [SerializeField] private int damage = 50;
    [SerializeField] private LayerMask enemyLayer;

    private Animator animator;
    private bool hasExploded = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("⚠️ Không tìm thấy Animator trên Bomb.");
        }

        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Gây damage cho enemy trong vùng nổ
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(damage);
        }

        // Phát animation nổ
        if (animator != null)
        {
            animator.Play("Explosion");
        }

        // Hủy bomb sau thời gian clip nổ (nếu không dùng Animation Event)
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        // Đợi đúng độ dài animation (ví dụ: 0.8 giây)
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

    // Nếu bạn gọi từ Animation Event ở cuối clip Explosion thì gọi hàm này
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
