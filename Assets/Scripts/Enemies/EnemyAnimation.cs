using UnityEngine;

[RequireComponent(typeof (EnemyMovement))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    private Transform playerTransform;
    private float minDistToPlayer;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private void Start()
    {
        if (!TryGetComponent(out anim))
        {
            Debug.LogError("No Animator on Enemy" + gameObject.name);
        }
        
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        minDistToPlayer = GetComponent<EnemyMovement>().GetMinDistToPlayer();
    }
    
    private void Update()
    {
        Vector2 moveDirection = playerTransform.position - transform.position;

        if (Vector3.Distance(playerTransform.position, transform.position) > minDistToPlayer)
        {
            anim.SetFloat(Horizontal, moveDirection.x);
            anim.SetFloat(Vertical, moveDirection.y);
            anim.SetFloat(Speed, moveDirection.sqrMagnitude);
        }
        else
        {
            anim.SetFloat(Speed, 0f);
        }

        if ((moveDirection.x < 0 && transform.localScale.x > 0) || 
            (moveDirection.x > 0 && transform.localScale.x < 0))
        {
            var localScale = transform.localScale;
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }
}
