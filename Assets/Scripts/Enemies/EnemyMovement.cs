using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToOtherEnemies;
    
    private Rigidbody2D rb;
    private Transform playerTransform;
    private EnemyInfo info;

    private void Start()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody2D on Enemy" + gameObject.name);
        }

        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        info = GetComponent<EnemyInfo>();
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        
        // Do not move if enemy is too close or too far away
        if (distanceToPlayer < info.MinDistanceToPlayer || distanceToPlayer > info.TriggerDistance)
        {
            info.MoveDirection = Vector2.zero;
            return;
        }

        // Stopping enemy when there is an obstacle on the way to player
        if (!info.IsThereObstacleBetweenMeAndPlayer())
        {
            info.MoveDirection = playerTransform.position - transform.position;
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * info.MoveDirection.normalized);
        }
        else
        {
            info.MoveDirection = Vector2.zero;
        }
        
        // Collisions with other enemies
        foreach (var col in Physics2D.OverlapCircleAll(transform.position, minDistanceToOtherEnemies))
        {
            if (!col.TryGetComponent<EnemyInfo>(out _) || col.gameObject == gameObject) continue;

            var enPos = col.transform.position;
            var myPos = transform.position;
            var distanceBetween = Vector2.Distance(enPos, myPos);
            var direction = new Vector2(myPos.x - enPos.x, myPos.y - enPos.y).normalized;
            rb.MovePosition(rb.position + Time.fixedDeltaTime * direction);
        }
    }
}
