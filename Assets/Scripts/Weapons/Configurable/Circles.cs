using System.Collections;
using UnityEngine;

public class Circles : ConfigurableWeapon
{
    [SerializeField] private int howManyCircles;
    [SerializeField] private int howManyBullets;
    [SerializeField] private float timeBetweenCircles;

    protected override void SpawnBullets() => StartCoroutine(SpawnCircles());

    private IEnumerator SpawnCircles()
    {
        for (int j = 0; j < howManyCircles; j++)
        {
            Bullet[] bullets = new Bullet[howManyBullets];
            float currentAngle = 0f;
            float differenceAngle = 360f / howManyBullets;
        
            for (int i = 0; i < howManyBullets; i++)
            {
                float x = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
                float y = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
                Vector2 temp = new Vector2(x, y);

                Vector3 myPos = transform.position;
                Bullet bullet = Instantiate(bulletPrefab, myPos, Quaternion.identity).GetComponent<Bullet>();
                bullet.Initialize(temp + (Vector2)myPos, damage, bulletSpeed);
                bullets[i] = bullet;
                currentAngle += differenceAngle;
            }
            Singleton.Instance.BulletManager.ConfigureBullets(bullets, false);
            yield return new WaitForSeconds(timeBetweenCircles);
        }
    }
}
