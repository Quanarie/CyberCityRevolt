using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyWeaponInput : WeaponInput
{
    private Transform _playerTransform;
    private EnemyInfo _info;
    
    private Vector2 _target;

    private void Start()
    {
        _playerTransform = Singleton.Instance.PlayerData.Player.transform;
        _info = GetComponent<EnemyInfo>();
    }

    private void Update()
    {
        Vector3 plPos = _playerTransform.position;

        if (Vector3.Distance(plPos, transform.position) > _info.TriggerDistance) return;
        
        _target = plPos;
        Shoot?.Invoke(_target);
    }

    public override Vector2 GetTarget() => _target;
}