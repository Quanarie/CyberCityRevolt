using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    
    private Vector2 input;
    
    private void Start()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody2D on Player");
        }
    }

    private void FixedUpdate() 
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * input.normalized);
    }

    // New Unity input system (calls this when WASD pressed)
    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }
}
