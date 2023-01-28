using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public void SpawnSingleBullet(Vector2 target, WeaponInfo info, out Bullet[] toSave)
    {
        toSave = new Bullet[1];
        toSave[0] = Instantiate(info.bulletPrefab, info.shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        toSave[0].Initialize(target, info.damage, info.bulletSpeed);
    }
    
    // Better to pass odd numbers
    public void SpawnCircleOfBullets(Vector2 target, WeaponInfo info, int quantity, float angle, out Bullet[] toSave)   
    {
        toSave = new Bullet[quantity];
        var weaponPos = info.shootPoint.position;
        var radius = Vector2.Distance(weaponPos, target);
        var angleBetweenMeAndTarget = -Vector2.SignedAngle(Vector2.up, target - (Vector2)weaponPos);
        var currentAngle = angleBetweenMeAndTarget - angle / 2;
        var differenceAngle = angle / (quantity - 1);
        
        for (int i = 0; i < quantity; i++)
        {
            var x = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius + weaponPos.x;
            var y = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius + weaponPos.y;
            var temp = new Vector2(x, y);

            toSave[i] = Instantiate(info.bulletPrefab, info.shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            toSave[i].Initialize(temp, info.damage, info.bulletSpeed);
            currentAngle += differenceAngle;
        }
    }
}
