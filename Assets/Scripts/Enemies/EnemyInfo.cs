using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [field : SerializeField] public float MinDistanceToPlayer { get; set; }
    [field : SerializeField] public float TriggerDistance { get; set; }
    
    public Vector2 MoveDirection { get; set; }

    public bool IsThereObstacleBetweenMeAndPlayer()
    {
        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        var enemyPos = transform.position;
        
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
}
