using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    [field : SerializeField] public GameObject Player { get; private set; }
    public PlayerInput Input { get; private set; }
    public PlayerHealth Health { get; private set; }

    private void Awake()
    {
        Input = Player.GetComponent<PlayerInput>();
        Health = Player.GetComponent<PlayerHealth>();
    }
}
