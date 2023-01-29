using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    public static readonly UnityEvent FlippedSide = new();
    
    private Transform _tf;
    private Animator _anim;
    
    private Vector2 input;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Roll = Animator.StringToHash("Roll");
    private static readonly int Blank = Animator.StringToHash("Blank");
    private static readonly int Die = Animator.StringToHash("Die");

    private void Start()
    {
        _tf = transform;
        
        if (!TryGetComponent(out _anim))
        {
            Debug.LogError("No Animator on Player");
        }

        // !! In the end of animations animator calls other methods on Player
        // Respawn in PlayerHealth
        Singleton.Instance.PlayerData.Health.Dying.AddListener(() => _anim.SetTrigger(Die));
        // EndRolling in PlayerMovement
        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(() => _anim.SetTrigger(Roll));
        // EndBlanking in PlayerBlank
        Singleton.Instance.PlayerData.Blank.StartedBlank.AddListener(() => _anim.SetTrigger(Blank));
    }
    
    private void Update()
    {
        // Checking if magnitude is non-zero, so after stopping there is at least
        // one non-zero coordinate to know in which direction to play idle animation
        if (input.magnitude != 0)
        {
            _anim.SetFloat(Horizontal, input.x);
            _anim.SetFloat(Vertical, input.y);
        }
        _anim.SetFloat(Speed, input.sqrMagnitude);
        
        // Flipping player, so it is not needed to create separate left and right animations
        if ((input.x < 0 && _tf.localScale.x > 0) || 
            (input.x > 0 && _tf.localScale.x < 0))
        {
            Vector3 localScale = _tf.localScale;
            _tf.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            FlippedSide?.Invoke();
        }
    }
    
    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }
}