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
    
    protected override void ShootContainer()
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        Shoot();
    }

    protected override void Shoot()
    {
        if (Random.value > 0.5f)
        {
            StartCoroutine(SpawnLasersWithDelay());
        }
        else
        {
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(input.GetTarget(), info, quantityOfBullets1, 
                spreadAngle, out spawnedBullets);
            ConfigureAfterShot();
        }
    }

    IEnumerator SpawnLasersWithDelay()
    {
        for (int i = 0; i < quantityOfWaves; i++)
        {
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(input.GetTarget(), info, 
                quantityOfBullets2, spreadAngle, out spawnedBullets);
            ConfigureAfterShot();
            yield return new WaitForSeconds(delayBetweenCircles);
        }
    }
}
