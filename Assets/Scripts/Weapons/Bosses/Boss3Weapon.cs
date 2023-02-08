using System.Collections;
using UnityEngine;

public class Boss3Weapon : Weapon
{
    [Header("1st attack type")]
    [SerializeField] private int quantityOfBullets;
    [SerializeField] private float delayBetweenBulletsInLaser;
    
    protected override void ShootContainer(Vector2 whereToAim)
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        Shoot(whereToAim);
    }
    
    protected override void Shoot(Vector2 whereToAim)
    {
        StartCoroutine(SpawnLaserWithDelay(whereToAim));
    }

    IEnumerator SpawnLaserWithDelay(Vector2 whereToAim)
    {
        for (int i = 0; i < quantityOfBullets; i++)
        {
            Singleton.Instance.BulletSpawner.SpawnSingleBullet(whereToAim, info, out spawnedBullets);
            ConfigureAfterShot();
            yield return new WaitForSeconds(delayBetweenBulletsInLaser);
        }
    }
}
