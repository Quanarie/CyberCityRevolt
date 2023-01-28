using UnityEngine;

public class CircleWeapon : Weapon
{
    [SerializeField] private int bulletQuantity;
    [SerializeField] private float spreadAngle;

    protected override void Shoot(Vector2 whereToAim)
    {
        Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(whereToAim, info, transform.position, 
            bulletQuantity, spreadAngle, out spawnedBullets);
    }
}