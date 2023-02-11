using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform route;
    [SerializeField] private float minWaitOnPointTime;
    [SerializeField] private float maxWaitOnPointTime;
    
    private Rigidbody2D rb;
    private Transform playerTransform;
    private EnemyInfo info;
    private Vector3[] routePoints;
    private int currentRoutePoint = 0;
    private const float MINIMAL_DISTANCE_TO_POINT = 0.1f;
    private bool isWaiting = false;

    private void Start()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody2D on Enemy" + gameObject.name);
        }

        playerTransform = Singleton.Instance.PlayerData.Player.transform;
        info = GetComponent<EnemyInfo>();

        if (route == null || route.childCount <= 1)
        {
            routePoints = new[] { transform.position };
            return;
        }
        
        routePoints = new Vector3[route.childCount];
        for (int i = 0; i < route.childCount; i++)
        {
            routePoints[i] = route.GetChild(i).position;
        }
    }

    private void FixedUpdate()
    {
        Vector3 plPos = playerTransform.position;

        float distanceToPlayer = Vector3.Distance(plPos, transform.position);

        if (distanceToPlayer > info.TriggerDistance || info.IsThereObstacleBetweenMeAndPlayer())
        {
            MoveOnRoute();
        }
        else if (distanceToPlayer > info.MinDistanceToPlayer)
        {
            MoveTo(plPos);
        }
        else
        {
            info.MoveDirection = Vector2.zero;
        }
    }
    
    private void MoveOnRoute()
    {
        if (route == null)
        {
            info.MoveDirection = Vector2.zero;
            return;
        }
        
        Vector3 moveToPoint = routePoints[currentRoutePoint];
        if (Vector3.Distance(moveToPoint, transform.position) < MINIMAL_DISTANCE_TO_POINT)
        {
            if (isWaiting) return;
            StartCoroutine(WaitAndSetNextPoint());
            return;
        }
        
        MoveTo(moveToPoint);
    }
    
    private void MoveTo(Vector3 destination)
    {
        info.MoveDirection = destination - transform.position;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * info.MoveDirection.normalized);
    }

    IEnumerator WaitAndSetNextPoint()
    {
        isWaiting = true;
        info.MoveDirection = Vector2.zero;
        yield return new WaitForSeconds(Random.Range(minWaitOnPointTime, maxWaitOnPointTime));
        isWaiting = false;
        SetNextRoutePoint();
    }

    private void SetNextRoutePoint()
    {
        if (currentRoutePoint == routePoints.Length - 1)
        {
            currentRoutePoint = 0;
            return;
        }
        currentRoutePoint++;
    }
    
    public bool CanBeTriggered()
    {
        Vector3 plPos = playerTransform.position;
        Vector3 closestPointOfRouteToPlayer = transform.position;
        if (route != null)
        {
            for (int i = 0; i < routePoints.Length; i++)
            {
                if (Vector3.Distance(routePoints[i], plPos) <
                    Vector3.Distance(closestPointOfRouteToPlayer, plPos))
                {
                    closestPointOfRouteToPlayer = routePoints[i];
                }
            }
        }
        
        float distanceToPlayer = Vector3.Distance(closestPointOfRouteToPlayer, plPos);
        return distanceToPlayer < info.TriggerDistance;
    }
}
