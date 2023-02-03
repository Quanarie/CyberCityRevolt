using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [field : SerializeField] public float MinDistanceToPlayer { get; set; }
    [field : SerializeField] public float TriggerDistance { get; set; }
    
    public Vector2 MoveDirection { get; set; }
}
