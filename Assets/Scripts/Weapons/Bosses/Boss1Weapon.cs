using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns N circles of bullets at one shot with delay between them
/// </summary>
public class Boss1Weapon : Weapon
{
    [SerializeField] private int minBulletsQuantityInOnCircle;
    [SerializeField] private int maxBulletsQuantityInOnCircle;
    [SerializeField] private int quantityOfCircles;
    [SerializeField] private float delayBetweenCircles;

    protected override void ShootContainer()
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        Shoot();
    }

    protected override void Shoot()
    {
        StartCoroutine(SpawnCirclesWithDelay());
    }

    IEnumerator SpawnCirclesWithDelay()
    {
        for (int i = 0; i < quantityOfCircles; i++)
        {
            var randQuantityOfBullets = Random.Range(minBulletsQuantityInOnCircle, maxBulletsQuantityInOnCircle);
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(input.GetTarget(), info, 
                randQuantityOfBullets, 360f, out spawnedBullets);
            ConfigureAfterShot();
            yield return new WaitForSeconds(delayBetweenCircles);
        }
    }
}