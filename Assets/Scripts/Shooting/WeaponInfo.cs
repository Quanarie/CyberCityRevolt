using System;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public int damage;
    public float bulletSpeed;
    public float rechargeTime;
    [Header("Just for info display")]
    public string name;
    public int amountOfBullets;
}