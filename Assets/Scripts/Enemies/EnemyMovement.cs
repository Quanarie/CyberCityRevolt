using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float minDistanceToPlayer;
    [SerializeField] private float speed;
    
    private Rigidbody2D rb;
    private Transform playerTransform;
    private Vector2 moveDirection;
    
    private void Start()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody2D on Enemy" + gameObject.name);
        }

        playerTransform = Singleton.Instance.PlayerData.Player.transform;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) < minDistanceToPlayer)
        {
            moveDirection = Vector2.zero;
            return;
        }

        // Stopping enemy when there is an obstacle on the way to player
        if (!IsThereObstacleBetweenEnemyAndPlayer(transform.position))
        {
            moveDirection = playerTransform.position - transform.position;
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * moveDirection.normalized);
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    public static bool IsThereObstacleBetweenEnemyAndPlayer(Vector3 enemyPos)
    {
        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        
        foreach (var hit in Physics2D.RaycastAll(enemyPos, plPos - enemyPos))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collision"))
            {
                return true;
            }
            else if (hit.collider.gameObject == Singleton.Instance.PlayerData.Player)
            {
                return false;
            }
        }

        return false;
    }

    public Vector2 GetMoveDirection() => moveDirection;
}
