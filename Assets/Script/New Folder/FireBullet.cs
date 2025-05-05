using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    [SerializeField] private float customSpeed = 20f;  // Tốc độ riêng cho FireBullet
    [SerializeField] private int customDamage = 10;    // Damage riêng cho FireBullet

    private void Awake()
    {
        // Cập nhật tốc độ và damage cho FireBullet
        speed = customSpeed;
        damage = customDamage;
    }
}
