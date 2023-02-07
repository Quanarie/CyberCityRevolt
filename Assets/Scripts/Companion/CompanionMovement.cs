using UnityEngine;

public class CompanionMovement : MonoBehaviour
{
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float smoothTime;

    private void FixedUpdate()
    {
        Vector3 plPos = Singleton.Instance.PlayerData.Player.transform.position;
        Vector3 myPos = transform.position;
        if (Vector3.Distance(plPos, myPos) < distanceToPlayer) return;
        
        Vector3 vel = Vector3.zero;
        transform.position = Vector3.SmoothDamp(myPos, plPos, 
            ref vel, smoothTime);
    }
}
