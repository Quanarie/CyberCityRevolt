using System;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
    public string name;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public int damage;
    public float bulletSpeed;
    public float rechargeTime;
}