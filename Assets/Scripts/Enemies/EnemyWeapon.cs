using UnityEngine;

public class EnemyWeapon : Weapon
{
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
    }

    protected override void Update()
    {
        base.Update();
        
        var plPos = playerTransform.position;
        
        if (!EnemyMovement.IsThereObstacleBetweenEnemyAndPlayer(transform.position))
        {
            RotateWeapon(plPos);
            Shoot(plPos);
        }
    }

    protected override void Shoot(Vector3 whereToAim)
    {
        if (timeElapsedFromLastShot > rechargeTime)
        {
            var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
            bullet.Initialize(whereToAim, damage, bulletSpeed);
            timeElapsedFromLastShot = 0f;
        }
    }
}
