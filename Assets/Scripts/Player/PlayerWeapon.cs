using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    protected override void Update()
    {
        base.Update();
        
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosInWorld = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
        
        RotateWeapon(mousePosInWorld);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot(mousePosInWorld);
        }
    }

    protected override void Shoot(Vector3 whereToAim)
    {
        if (timeElapsedFromLastShot > rechargeTime)
        {
            var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
            bullet.Initialize(whereToAim, damage, bulletSpeed);
            timeElapsedFromLastShot = 0f;
        }
    }
}
