using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : Bullet
{
    [SerializeField] private float customSpeed = 15f;  // Tốc độ riêng cho MagicBullet
    [SerializeField] private int customDamage = 35;    // Damage riêng cho MagicBullet

    private void Awake()
    {
        // Cập nhật tốc độ và damage cho MagicBullet
        speed = customSpeed;
        damage = customDamage;
    }
}
