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

    protected override void ShootContainer(Vector2 whereToAim)
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        Shoot(whereToAim);
    }

    protected override void Shoot(Vector2 whereToAim)
    {
        StartCoroutine(SpawnCirclesWithDelay(whereToAim));
    }

    IEnumerator SpawnCirclesWithDelay(Vector2 whereToAim)
    {
        for (int i = 0; i < quantityOfCircles; i++)
        {
            var randQuantityOfBullets = Random.Range(minBulletsQuantityInOnCircle, maxBulletsQuantityInOnCircle);
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(whereToAim, info, 
                randQuantityOfBullets, 360f, out spawnedBullets);
            ConfigureAfterShot();
            yield return new WaitForSeconds(delayBetweenCircles);
        }
    }
}