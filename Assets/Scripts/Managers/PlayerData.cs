using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    [field : SerializeField] public GameObject Player { get; private set; }
    public PlayerInput Input { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerHealth Health { get; private set; }
    public PlayerBlank Blank { get; private set; }
    
    // There is no weapon here, because it is not constant
    private void Awake()
    {
        Input = Player.GetComponent<PlayerInput>();
        Movement = Player.GetComponent<PlayerMovement>();
        Health = Player.GetComponent<PlayerHealth>();
        Blank = Player.GetComponent<PlayerBlank>();
    }
}
