using System;
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

    private float timeFromLastRoll;

    private Rigidbody2D rb;
    
    private Vector2 input;
    private Vector2 nonZeroInput;
    private Vector2 rollingInput;
    private bool isRolling;
    
    private void Start()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody2D on Player");
        }
        
        Singleton.Instance.PlayerData.Blank.StartedBlank.AddListener(EndRollIfStartedBlank);
    }

    private void FixedUpdate()
    {
        if (isRolling)
        {
            rb.MovePosition(rb.position + rollingSpeed * Time.fixedDeltaTime * rollingInput.normalized);
        }
        else
        {
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * input.normalized);
        }

        timeFromLastRoll += Time.fixedDeltaTime;
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
        if (isRolling || timeFromLastRoll < rollRechargeTime) return;

        StartedRolling?.Invoke();
        isRolling = true;
        rollingInput = nonZeroInput;
        timeFromLastRoll = 0f;
    }

    private void EndRollIfStartedBlank()
    {
        if (!isRolling) return;
        
        EndRolling();
    }
    
    // !! Called by animator !!
    public void EndRolling()
    {
        isRolling = false;
        EndedRolling?.Invoke();
    }
}
