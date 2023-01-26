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

    protected override void Shoot(Vector2 whereToAim)
    {
        StartCoroutine(SpawnWithDelay(whereToAim));
    }

    IEnumerator SpawnWithDelay(Vector2 target)
    {
        for (int i = 0; i < quantityOfCircles; i++)
        {
            var randQuantityOfBullets = Random.Range(minBulletsQuantityInOnCircle, maxBulletsQuantityInOnCircle);
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(target, bulletPrefab, damage, bulletSpeed, 
                shootPoint.position, randQuantityOfBullets, 360f, out spawnedBullets);
            SetLayerBullets();
            yield return new WaitForSeconds(delayBetweenCircles);
        }
    }
}