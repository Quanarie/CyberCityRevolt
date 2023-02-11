using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyWeaponInput : WeaponInput
{
    private Transform playerTransform;
    private EnemyInfo info;
    
    private Vector2 target;

    private void Start()
    {
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        info = GetComponent<EnemyInfo>();
    }

    protected virtual void Update()
    {
        Vector3 plPos = playerTransform.position;

        if (Vector3.Distance(plPos, transform.position) > info.TriggerDistance) return;
        target = plPos;
        Shoot?.Invoke(target);
    }

    public override Vector2 GetWhereToAim() => target;
}