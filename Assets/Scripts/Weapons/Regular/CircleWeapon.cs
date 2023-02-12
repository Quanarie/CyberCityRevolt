using UnityEngine;

public class CircleWeapon : Weapon
{
    [SerializeField] private int bulletQuantity;
    [SerializeField] private float spreadAngle;

    protected override void SpawnBullets(Vector2 whereToAim)
    {
        spawnedBullets = new Bullet[bulletQuantity];
        Vector2 weaponPos = info.shootPoint.position;
        float radius = Vector2.Distance(weaponPos, whereToAim);
        float angleBetweenMeAndTarget = -Vector2.SignedAngle(Vector2.up, whereToAim - (Vector2)weaponPos);
        float currentAngle = angleBetweenMeAndTarget - spreadAngle / 2;
        float differenceAngle = spreadAngle / (bulletQuantity - 1);
        
        for (int i = 0; i < bulletQuantity; i++)
        {
            float x = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius + weaponPos.x;
            float y = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius + weaponPos.y;
            Vector2 temp = new Vector2(x, y);

            spawnedBullets[i] = Instantiate(info.bulletPrefab, info.shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            spawnedBullets[i].Initialize(temp, info.damage, info.bulletSpeed);
            currentAngle += differenceAngle;
        }
    }
}