using UnityEngine;

public class Boss3WeaponInput : EnemyWeaponInput
{
    [SerializeField] private float predictableShootCoef;
    
    protected override void Update()
    {
        Vector3 plPos = playerTransform.position;
        
        if (Vector3.Distance(plPos, transform.position) > info.TriggerDistance) return;

        target = (Vector2)plPos + Singleton.Instance.PlayerData.Movement.GetOffsetToShoot() * predictableShootCoef;
        Shoot?.Invoke();
    }
}