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

    private void Update()
    {
        var plPos = playerTransform.position;

        if (Vector3.Distance(plPos, transform.position) > info.TriggerDistance) return;

        if (info.IsThereObstacleBetweenMeAndPlayer()) return;
        
        target = plPos;
        Shoot?.Invoke(target);
    }

    public override Vector2 GetTarget() => target;
}