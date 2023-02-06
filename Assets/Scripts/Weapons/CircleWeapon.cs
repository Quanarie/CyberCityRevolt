using UnityEngine;

public class CircleWeapon : Weapon
{
    [SerializeField] private int bulletQuantity;
    [SerializeField] private float spreadAngle;

    protected override void Shoot()
    {
        Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(input.GetTarget(), info, 
            bulletQuantity, spreadAngle, out spawnedBullets);
    }
}