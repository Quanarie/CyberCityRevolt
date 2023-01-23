using UnityEngine;

[RequireComponent(typeof (EnemyInfo))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    private EnemyInfo info;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private void Start()
    {
        if (!TryGetComponent(out anim))
        {
            Debug.LogError("No Animator on Enemy" + gameObject.name);
        }
        
        info = GetComponent<EnemyInfo>();
    }
    
    private void Update()
    {
        if (info.MoveDirection.sqrMagnitude != 0)
        {
            anim.SetFloat(Horizontal, info.MoveDirection.x);
            anim.SetFloat(Vertical, info.MoveDirection.y);
        }
        anim.SetFloat(Speed, info.MoveDirection.sqrMagnitude);

        if ((info.MoveDirection.x < 0 && transform.localScale.x > 0) || 
            (info.MoveDirection.x > 0 && transform.localScale.x < 0))
        {
            var localScale = transform.localScale;
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }
}
