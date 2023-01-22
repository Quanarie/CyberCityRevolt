using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float interpolationRate;   // 0 - none, 1 - instant
    
    private Transform playerTransform;
    private Camera mainCam;

    private void Start()
    {
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        var plPos = playerTransform.position;
        var mousePosOnScreen = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var camPosOnPlayer = new Vector3(plPos.x, plPos.y, mainCam.transform.position.z);
        var mousePosInWorld = new Vector3(mousePosOnScreen.x, mousePosOnScreen.y, transform.position.z);
        transform.position = Vector3.Lerp(camPosOnPlayer, mousePosInWorld, interpolationRate);
    }
}