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
    
    public override void DropWeapon()
    {
        StopAllCoroutines();
        transform.SetParent(null);
        input.Shoot.RemoveListener(ShootContainer);
        isDropped = true;
    }
    
    protected override void Shoot(Vector2 whereToAim)
    {
        StartCoroutine(SpawnCirclesWithDelay(whereToAim));
    }

    IEnumerator SpawnCirclesWithDelay(Vector2 target)
    {
        for (int i = 0; i < quantityOfCircles; i++)
        {
            var randQuantityOfBullets = Random.Range(minBulletsQuantityInOnCircle, maxBulletsQuantityInOnCircle);
            Singleton.Instance.BulletSpawner.SpawnCircleOfBullets(target, info, 
                randQuantityOfBullets, 360f, out spawnedBullets);
            SetLayerBullets();
            OrientBullets();
            if (isOnPlayer) cam.Shake(cameraShakeDuration, cameraShakeMagnitude);
            timeElapsedFromLastShot = 0f;
            yield return new WaitForSeconds(delayBetweenCircles);
        }
    }
}