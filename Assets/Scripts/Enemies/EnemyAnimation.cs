using UnityEngine;

[RequireComponent(typeof (EnemyMovement))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    private EnemyMovement movement;
    private Vector2 moveDirection;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private void Start()
    {
        if (!TryGetComponent(out anim))
        {
            Debug.LogError("No Animator on Enemy" + gameObject.name);
        }
        
        movement = GetComponent<EnemyMovement>();
    }
    
    private void Update()
    {
        moveDirection = movement.GetMoveDirection();

        if (moveDirection.sqrMagnitude != 0)
        {
            anim.SetFloat(Horizontal, moveDirection.x);
            anim.SetFloat(Vertical, moveDirection.y);
        }
        anim.SetFloat(Speed, moveDirection.sqrMagnitude);

        if ((moveDirection.x < 0 && transform.localScale.x > 0) || 
            (moveDirection.x > 0 && transform.localScale.x < 0))
        {
            var localScale = transform.localScale;
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }
}
