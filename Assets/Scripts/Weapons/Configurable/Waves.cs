using System.Collections;
using UnityEngine;

public class Waves : ConfigurableWeapon
{
    [SerializeField] private int howManyBullets;
    [SerializeField] private float howManySinPeriods;
    [SerializeField] private float timeBetweenBullets;
    [SerializeField] private float angle;
    
    protected override void SpawnBullets() => StartCoroutine(SpawnSpiral());
        
    private IEnumerator SpawnSpiral()
    {
        float x = 0f;
        for (int i = 0; i < howManyBullets; i++)
        {
            x += 2 * Mathf.PI * howManySinPeriods / howManyBullets;
            float y = x * Mathf.Sin(x);
            float angleInRadians = Mathf.Deg2Rad * angle;
            Vector2 temp = new Vector2(x * Mathf.Cos(angleInRadians) - y * Mathf.Sin(angleInRadians),
                x * Mathf.Sin(angleInRadians) + y * Mathf.Cos(angleInRadians));

            Vector3 myPos = transform.position;
            Bullet bullet = Instantiate(bulletPrefab, myPos, Quaternion.identity).GetComponent<Bullet>();
            bullet.Initialize(temp + (Vector2)myPos, damage, bulletSpeed);
            Singleton.Instance.BulletManager.ConfigureBullets(new[] { bullet }, false);

            yield return new WaitForSeconds(timeBetweenBullets);
        }
    }
}