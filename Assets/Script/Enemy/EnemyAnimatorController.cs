using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    public Animator animator;
    private Vector3 lastPosition;
    private string lastDirection = "";

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        lastPosition = transform.position;
    }

    void Update()
    {
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        Vector2 moveDir = new Vector2(transform.position.x - lastPosition.x, transform.position.y - lastPosition.y);
        lastPosition = transform.position;

        if (moveDir.magnitude > 0.01f)
        {
            Vector2 dir = moveDir.normalized;

            Vector2[] directions = {
                Vector2.up,     // ToLen
                Vector2.down,   // ToXuong
                Vector2.left,   // ToTrai
                Vector2.right   // ToPhai
            };

            string[] directionNames = {
                "ToLen",
                "ToXuong",
                "ToTrai",
                "ToPhai"
            };

            float maxDot = -1f;
            int bestIndex = -1;

            for (int i = 0; i < directions.Length; i++)
            {
                float dot = Vector2.Dot(dir, directions[i]);
                if (dot > maxDot)
                {
                    maxDot = dot;
                    bestIndex = i;
                }
            }

            string newDirection = directionNames[bestIndex];

            if (newDirection != lastDirection)
            {
                if (!string.IsNullOrEmpty(lastDirection))
                    animator.ResetTrigger(lastDirection);

                animator.SetTrigger(newDirection);
                lastDirection = newDirection;

             //   Debug.Log("▶ Enemy hướng: " + newDirection);
            }
        }
    }
}
