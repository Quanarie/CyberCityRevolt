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

    private void Start()
    {
        tf = transform;
        
        if (!TryGetComponent(out anim))
        {
            Debug.LogError("No Animator on Player");
        }

        Singleton.Instance.PlayerData.Health.Dying.AddListener(PlayDeathAnimation);
        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(PlayRollingAnimation);
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

    // !! In the end of animation animator calls Respawn() in PlayerHealth script !!
    public void PlayDeathAnimation()
    {
        anim.SetTrigger(Die);
    }
    
    public void PlayRollingAnimation()
    {
        anim.SetTrigger(Roll);
    }
}