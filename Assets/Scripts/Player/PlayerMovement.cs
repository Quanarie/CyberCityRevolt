using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public UnityEvent StartedRolling;
    [HideInInspector] public UnityEvent EndedRolling;

    [SerializeField] private float speed;
    [SerializeField] private float rollingSpeed;
    [SerializeField] private float rollRechargeTime;

    private float _timeFromLastRoll;

    private Rigidbody2D _rb;
    
    private Vector2 input;
    private Vector2 nonZeroInput = Vector2.up;
    private Vector2 rollingInput;
    private bool isRolling;
    
    private void Start()
    {
        if (!TryGetComponent(out _rb))
        {
            Debug.LogError("No Rigidbody2D on Player");
        }
        
        Singleton.Instance.PlayerData.Blank.StartedBlank.AddListener(EndRollIfStartedOtherAnimation);
        Singleton.Instance.PlayerData.Health.Dying.AddListener(EndRollIfStartedOtherAnimation);
    }

    private void FixedUpdate()
    {
        if (isRolling)
        {
            _rb.MovePosition(_rb.position + rollingSpeed * Time.fixedDeltaTime * rollingInput.normalized);
        }
        else
        {
            _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * input.normalized);
        }

        _timeFromLastRoll += Time.fixedDeltaTime;
    }

    // New Unity input system (calls this when WASD pressed)
    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        
        if (input == Vector2.zero) return;
        
        nonZeroInput = input;
    }
    
    public void OnRoll(InputValue value)
    {
        if (isRolling || _timeFromLastRoll < rollRechargeTime) return;

        StartedRolling?.Invoke();
        isRolling = true;
        rollingInput = nonZeroInput;
        _timeFromLastRoll = 0f;
    }

    private void EndRollIfStartedOtherAnimation()
    {
        if (!isRolling) return;

        EndRoll();
    }
    
    // !! Called by animator !!
    public void EndRoll()
    {
        if (!isRolling) return;
        
        isRolling = false;
        EndedRolling?.Invoke();
    }

    public Vector2 GetOffsetToShoot()
    {
        return input * speed;
    }
}
