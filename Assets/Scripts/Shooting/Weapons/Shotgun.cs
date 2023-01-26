using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int howManyBulletsAtOnce;
    [SerializeField] private float spreadAngle;

    protected override void Shoot(Vector2 whereToAim)
    {
        var pos = transform.position;
        spawnedBullets = new Bullet[howManyBulletsAtOnce];
        for (int i = 0; i < howManyBulletsAtOnce; i++)
        {
            var radius = Vector2.Distance(pos, whereToAim);
            var angleBetweenMeAndTarget = -Vector2.SignedAngle(Vector2.up, whereToAim - (Vector2)pos);
            var randomAngle = Random.Range(angleBetweenMeAndTarget - spreadAngle / 2,
                angleBetweenMeAndTarget + spreadAngle / 2);
            var x = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * radius + pos.x;
            var y = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * radius + pos.y;
            var temp = new Vector2(x, y);

            spawnedBullets[i] = Instantiate(bulletPrefab, shootPoint.position,
                Quaternion.identity).GetComponent<Bullet>();
            spawnedBullets[i].gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
            spawnedBullets[i].Initialize(temp, damage, bulletSpeed);
        }
    }
}