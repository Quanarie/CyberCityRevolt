using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private float constraint;
    
    private Camera mainCam;
    private Transform playerTransform;

    private void Start()
    {
        mainCam = Camera.main;
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
    }

    private void Update()
    {
        var mousePosInWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var plPos = playerTransform.position;
        var targetPosition = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
        targetPosition.x = Mathf.Clamp(targetPosition.x, -constraint + plPos.x, constraint + plPos.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -constraint + plPos.y, constraint + plPos.y);
        transform.position = targetPosition;
    }
}
