using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float lerpValue;
    [SerializeField] private float constraint;
    
    private Camera mainCam;
    private Transform playerTransform;

    private void Start()
    {
        mainCam = Camera.main;
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
    }

    private void LateUpdate()
    {
        var mousePosInWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var plPos = playerTransform.position;
        var targetPosition = mousePosInWorld;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -constraint + plPos.x, constraint + plPos.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -constraint + plPos.y, constraint + plPos.y);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpValue);
    }
}
