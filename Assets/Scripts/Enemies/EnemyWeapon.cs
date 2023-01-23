using UnityEngine;

public class EnemyWeapon : Weapon
{
    private Transform playerTransform;
    private EnemyInfo info;

    private void Start()
    {
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        if (!transform.parent.TryGetComponent(out info))
        {
            Debug.LogError("No EnemyInfo on Enemy" + transform.parent.name);
        }
    }

    protected override void Update()
    {
        base.Update();
        
        var plPos = playerTransform.position;

        if (Vector3.Distance(plPos, transform.position) > info.TriggerDistance)
        {
            return;
        }

        if (!info.IsThereObstacleBetweenMeAndPlayer())
        {
            RotateWeapon(plPos);
            if (timeElapsedFromLastShot > rechargeTime)
            {
                Shoot(plPos);
            }
        }
    }

    protected override void Shoot(Vector3 whereToAim)
    {
        var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
        bullet.Initialize(whereToAim, damage, bulletSpeed);
        timeElapsedFromLastShot = 0f;
    }
}
