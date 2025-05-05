using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill; // Thanh máu
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0); // Độ cao thanh máu

    private Transform target;
    private Camera mainCamera;

    void Start()
    {
        target = transform.parent; // Gán thanh máu vào Enemy
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.forward = mainCamera.transform.forward; // Giữ thanh máu luôn đối diện camera
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
       // Debug.Log("Health Bar Fill: " + healthBarFill.fillAmount);
    }
}
