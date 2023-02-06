using System.Collections;
using UnityEngine;

public class Boss3Weapon : Weapon
{
    [Header("1st attack type")]
    [SerializeField] private int quantityOfBullets;
    [SerializeField] private float delayBetweenBulletsInLaser;
    
    protected override void ShootContainer()
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        Shoot();
    }
    
    protected override void Shoot()
    {
        StartCoroutine(SpawnLaserWithDelay());
    }

    IEnumerator SpawnLaserWithDelay()
    {
        for (int i = 0; i < quantityOfBullets; i++)
        {
            Singleton.Instance.BulletSpawner.SpawnSingleBullet(input.GetTarget(), info, out spawnedBullets);
            ConfigureAfterShot();
            yield return new WaitForSeconds(delayBetweenBulletsInLaser);
        }
    }
}
