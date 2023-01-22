using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float minDistanceToPlayer;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Transform playerTransform;
    
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
        if (Vector3.Distance(playerTransform.position, transform.position) < minDistanceToPlayer) return;
        
        Vector2 moveDirection = playerTransform.position - transform.position;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * moveDirection.normalized);
    }

    public float GetMinDistToPlayer() => minDistanceToPlayer;
}
