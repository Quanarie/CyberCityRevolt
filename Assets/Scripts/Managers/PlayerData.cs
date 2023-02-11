using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    [field : SerializeField] public GameObject Player { get; private set; }
    public PlayerInput Input { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerHealth Health { get; private set; }
    public PlayerBlank Blank { get; private set; }
    public PlayerAnimation Animation { get; private set; }

    private InputAction move;
    private InputAction roll;
    private InputAction fire;
    private InputAction pickup;

    // There is no weapon here, because it is not constant
    private void Awake()
    {
        Input = Player.GetComponent<PlayerInput>();
        Movement = Player.GetComponent<PlayerMovement>();
        Health = Player.GetComponent<PlayerHealth>();
        Blank = Player.GetComponent<PlayerBlank>();
        Animation = Player.GetComponent<PlayerAnimation>();

        move = Input.actions.FindAction("Move");
        roll = Input.actions.FindAction("Roll");
        fire = Input.actions.FindAction("Fire");
        pickup = Input.actions.FindAction("Pickup");
    }

    public void DisableMoveInput()
    {
        move.Disable();
        roll.Disable();
        fire.Disable();
        pickup.Disable();
    }
    
    public void EnableMoveInput()
    {
        move.Enable();
        roll.Enable();
        fire.Enable();
        pickup.Enable();
    }
}
