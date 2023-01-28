using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "ScriptableObjects")]
public class WeaponInfo : ScriptableObject
{
    public Vector2 spawnOffset;
    public GameObject bulletPrefab;
    public int damage;
    public float bulletSpeed;
    public float rechargeTime;
}
