using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float smoothTime;
    [SerializeField] private float constraint;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;

    private Camera mainCam;
    private Transform playerTransform;
    
    private void Start()
    {
        mainCam = Camera.main;
        playerTransform = Singleton.Instance.PlayerData.Player.transform;
    }

    // FixedUpdate is stopped by setting Time.timeScale to 0f, unlike Update()
    private void FixedUpdate()
    {
        var mousePosInWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var plPos = playerTransform.position;
        var targetPosition = mousePosInWorld;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -constraint + plPos.x, constraint + plPos.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -constraint + plPos.y, constraint + plPos.y);
        var vel = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, 
            ref vel, smoothTime);
    }

    public void Shake() => StartCoroutine(ShakeCor());

    private IEnumerator ShakeCor()
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < cameraShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * cameraShakeMagnitude + orignalPosition.x;
            float y = Random.Range(-1f, 1f) * cameraShakeMagnitude + orignalPosition.y;

            transform.position = new Vector3(x, y, orignalPosition.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }
}