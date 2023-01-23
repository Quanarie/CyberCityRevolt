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
}