using UnityEngine;

[RequireComponent(typeof (EnemyInfo))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator _anim;
    private EnemyInfo _info;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private void Start()
    {
        if (!TryGetComponent(out _anim))
        {
            Debug.LogError("No Animator on Enemy" + gameObject.name);
        }
        
        _info = GetComponent<EnemyInfo>();
    }
    
    private void Update()
    {
        if (_info.MoveDirection.sqrMagnitude != 0)
        {
            _anim.SetFloat(Horizontal, _info.MoveDirection.x);
            _anim.SetFloat(Vertical, _info.MoveDirection.y);
        }
        _anim.SetFloat(Speed, _info.MoveDirection.sqrMagnitude);

        if ((_info.MoveDirection.x < 0 && transform.localScale.x > 0) || 
            (_info.MoveDirection.x > 0 && transform.localScale.x < 0))
        {
            Transform temp = transform;
            Vector3 localScale = temp.localScale;
            temp.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }
}
