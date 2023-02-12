using System.Collections;
using UnityEngine;

public class Spiral : ConfigurableWeapon
{
    [SerializeField] private bool isToTheRight;
    [SerializeField] private int howManyBullets;
    [SerializeField] private float timeBetweenBullets;
    [SerializeField] private float angleBetweenBullets;
    [SerializeField] private float startAngle;

    protected override void SpawnBullets() => StartCoroutine(SpawnSpiral());

    private IEnumerator SpawnSpiral()
    {
        float currentAngle = startAngle;
        for (int i = 0; i < howManyBullets; i++)
        {
            float x = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            float y = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            Vector2 temp = new Vector2(x, y);

            Vector3 myPos = transform.position;
            Bullet bullet = Instantiate(bulletPrefab, myPos, Quaternion.identity).GetComponent<Bullet>();
            bullet.Initialize(temp + (Vector2)myPos, damage, bulletSpeed);
            Singleton.Instance.BulletManager.ConfigureBullets(new[] { bullet }, false);
            currentAngle += isToTheRight ? angleBetweenBullets : -angleBetweenBullets;
            
            yield return new WaitForSeconds(timeBetweenBullets);
        }
    }
    
}