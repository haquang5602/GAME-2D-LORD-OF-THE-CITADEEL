using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHealth;
    public int goldReward;
    public float moveSpeed;
}
