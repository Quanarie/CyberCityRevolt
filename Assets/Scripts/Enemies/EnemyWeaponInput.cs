using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyWeaponInput : WeaponInput
{
    private Transform playerTransform;
    private EnemyInfo info;
    
    private Vector2 target;

    protected override void Start()
    {
        base.Start();
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        info = GetComponent<EnemyInfo>();
    }

    protected virtual void Update()
    {
        Vector3 plPos = playerTransform.position;

        if (Vector3.Distance(plPos, transform.position) > info.TriggerDistance 
            || info.IsThereObstacleBetweenMeAndPlayer()) return;
        target = plPos;
        Shoot?.Invoke(target);
    }

    public override Vector2 GetWhereToAim() => target;
}