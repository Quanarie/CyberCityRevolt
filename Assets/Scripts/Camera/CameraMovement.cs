using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float smoothTime;
    [SerializeField] private float constraint;

    private Camera _mainCam;
    private Transform _playerTransform;
    
    private void Start()
    {
        _mainCam = Camera.main;
        _playerTransform = Singleton.Instance.PlayerData.Player.transform;
    }

    // FixedUpdate is stopped by setting Time.timeScale to 0f, unlike Update()
    private void FixedUpdate()
    {
        Vector3 mousePosInWorld = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 plPos = _playerTransform.position;
        Vector3 targetPosition = mousePosInWorld;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -constraint + plPos.x, constraint + plPos.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -constraint + plPos.y, constraint + plPos.y);
        Vector3 vel = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, 
            ref vel, smoothTime);
    }

    public void Shake(float dur, float magn) => StartCoroutine(ShakeCor(dur, magn));

    private IEnumerator ShakeCor(float dur, float magn)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;
        
        while (elapsed < dur)
        {
            Vector3 myPos = transform.position;

            float x = Random.Range(-1f, 1f) * magn + myPos.x;
            float y = Random.Range(-1f, 1f) * magn + myPos.y;

            transform.position = new Vector3(x, y, myPos.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        transform.position = originalPos;
    }
}