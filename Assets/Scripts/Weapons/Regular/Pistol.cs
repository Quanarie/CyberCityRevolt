using UnityEngine;

public class Pistol : Weapon
{
    protected override void SpawnBullets(Vector2 whereToAim)
    {
        spawnedBullets = new Bullet[1];
        spawnedBullets[0] = Instantiate(info.bulletPrefab, info.shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        spawnedBullets[0].Initialize(whereToAim, info.damage, info.bulletSpeed);
    }
}