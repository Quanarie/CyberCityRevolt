using UnityEngine;

public class Pistol : Weapon
{
    protected override void Shoot(Vector2 whereToAim)
    {
        Singleton.Instance.BulletSpawner.SpawnSingleBullet(whereToAim, info, out spawnedBullets);
    }
}