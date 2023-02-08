using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyWeaponInput : WeaponInput
{
    protected Transform playerTransform;
    protected EnemyInfo info;
    
    protected Vector2 target;

    private bool isShootingWithPrediction = false;
    private const float MIN_TIME_TO_CHANGE_SHOOT_MODE = 1f;
    private const float MAX_TIME_TO_CHANGE_SHOOT_MODE = 3f;
    private const float MIN_PREDICTABLE_SHOOT_COEF = 0.5f;
    private const float MAX_PREDICTABLE_SHOOT_COEF = 0.9f;
    private float timeToChangeShootMode;
    private float predictableShootCoef;
    private  float elapsedSinceLastShootModeChange = 0f;

    private void Start()
    {
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        info = GetComponent<EnemyInfo>();
        timeToChangeShootMode = Random.Range(MIN_TIME_TO_CHANGE_SHOOT_MODE, MAX_TIME_TO_CHANGE_SHOOT_MODE);
    }

    protected virtual void Update()
    {
        elapsedSinceLastShootModeChange += Time.deltaTime;

        if (elapsedSinceLastShootModeChange > timeToChangeShootMode)
        {
            isShootingWithPrediction = !isShootingWithPrediction;
            predictableShootCoef = Random.Range(MIN_PREDICTABLE_SHOOT_COEF, MAX_PREDICTABLE_SHOOT_COEF);
            timeToChangeShootMode = Random.Range(MIN_TIME_TO_CHANGE_SHOOT_MODE, MAX_TIME_TO_CHANGE_SHOOT_MODE);
            elapsedSinceLastShootModeChange = 0f;
        }
        
        Vector3 plPos = playerTransform.position;

        if (Vector3.Distance(plPos, transform.position) > info.TriggerDistance) return;
        
        if (isShootingWithPrediction)
        {
            target = (Vector2)plPos +
                     Singleton.Instance.PlayerData.Movement.GetOffsetToShoot() * predictableShootCoef;
        }
        else
        {
            target = plPos;
        }
        Shoot?.Invoke(target);
    }

    public override Vector2 GetWhereToAim() => Singleton.Instance.PlayerData.Player.transform.position;
}