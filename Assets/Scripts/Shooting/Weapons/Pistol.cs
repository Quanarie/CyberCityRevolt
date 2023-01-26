using UnityEngine;

public class Pistol : Weapon
{
    protected override void Shoot(Vector2 whereToAim)
    {
        spawnedBullets = new Bullet[1];
        spawnedBullets[0] = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        spawnedBullets[0].Initialize(whereToAim, damage, bulletSpeed);
    }
}