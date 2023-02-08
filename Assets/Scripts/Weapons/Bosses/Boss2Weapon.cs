using System.Collections;
using UnityEngine;

public class Boss2Weapon : Weapon
{
    [SerializeField] private float spreadAngle;
    
    [Header("1st attack type")]
    [SerializeField] private int quantityOfBullets1;
    
    [Header("2nd attack type")]
    [SerializeField] private int quantityOfBullets2;
    [SerializeField] private int quantityOfWaves;
    [SerializeField] private float delayBetweenCircles;
    
    protected override void ShootContainer(Vector2 whereToAim)
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        Shoot(whereToAim);
    }

    protected override void Shoot(Vector2 whereToAim)
    {
        if (Random.value > 0.5f)
        {
            StartCoroutine(SpawnLasersWithDelay(whereToAim));
        }
        else
        {
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(whereToAim, info, quantityOfBullets1, 
                spreadAngle, out spawnedBullets);
            ConfigureAfterShot();
        }
    }

    IEnumerator SpawnLasersWithDelay(Vector2 whereToAim)
    {
        for (int i = 0; i < quantityOfWaves; i++)
        {
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(whereToAim, info, 
                quantityOfBullets2, spreadAngle, out spawnedBullets);
            ConfigureAfterShot();
            yield return new WaitForSeconds(delayBetweenCircles);
        }
    }
}
