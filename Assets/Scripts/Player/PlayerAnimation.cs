using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    private Transform tf;
    private Animator anim;
    
    private Vector2 input;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Roll = Animator.StringToHash("Roll");
    private static readonly int Blank = Animator.StringToHash("Blank");

    private void Start()
    {
        tf = transform;
        
        if (!TryGetComponent(out anim))
        {
            Debug.LogError("No Animator on Player");
        }

        // !! In the end of animations animator calls other methods on Player
        // Respawn in PlayerHealth
        Singleton.Instance.PlayerData.Health.Dying.AddListener(() => anim.SetTrigger(Die));
        // EndRolling in PlayerMovement
        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(() => anim.SetTrigger(Roll));
        // EndBlanking in PlayerBlank
        Singleton.Instance.PlayerData.Blank.StartedBlank.AddListener(() => anim.SetTrigger(Blank));
    }
    
    private void Update()
    {
        // Checking if magnitude is non-zero, so after stopping there is at least
        // one non-zero coordinate to know in which direction to play idle animation
        if (input.magnitude != 0)
        {
            anim.SetFloat(Horizontal, input.x);
            anim.SetFloat(Vertical, input.y);
        }
        anim.SetFloat(Speed, input.sqrMagnitude);
        
        // Flipping player, so it is not needed to create separate left and right animations
        if ((input.x < 0 && tf.localScale.x > 0) || 
            (input.x > 0 && tf.localScale.x < 0))
        {
            var localScale = tf.localScale;
            tf.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }
    
    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }
}