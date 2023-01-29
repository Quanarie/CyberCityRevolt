using System.Collections;
using Newtonsoft.Json.Bson;
using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToOtherEnemies;
    [SerializeField] private Transform route;
    [SerializeField] private float waitOnPointTime;
    
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
        CollideWithOtherEnemies();
        
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        
        if (distanceToPlayer < info.MinDistanceToPlayer || distanceToPlayer > info.TriggerDistance || 
            info.IsThereObstacleBetweenMeAndPlayer())
        {
            MoveOnRoute();
            return;
        }
        
       MoveTo(playerTransform.position);
    }

    private void MoveTo(Vector3 destination)
    {
        info.MoveDirection = destination - transform.position;
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * info.MoveDirection.normalized);
    }

    private void MoveOnRoute()
    {
        Vector3 moveToPoint = routePoints[currentRoutePoint];
        if (Vector3.Distance(moveToPoint, transform.position) < MINIMAL_DISTANCE_TO_POINT)
        {
            if (isWaiting) return;
            StartCoroutine(WaitAndSetNextPoint());
        }
        else
        {
            MoveTo(moveToPoint);
        }
    }

    IEnumerator WaitAndSetNextPoint()
    {
        isWaiting = true;
        info.MoveDirection = Vector2.zero;
        yield return new WaitForSeconds(waitOnPointTime);
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

    private void CollideWithOtherEnemies()
    {
        foreach (var col in Physics2D.OverlapCircleAll(transform.position, minDistanceToOtherEnemies))
        {
            if (!col.TryGetComponent<EnemyInfo>(out _) || col.gameObject == gameObject) continue;

            var enPos = col.transform.position;
            var myPos = transform.position;
            var distanceBetween = Vector2.Distance(enPos, myPos);
            var direction = new Vector2(myPos.x - enPos.x, myPos.y - enPos.y).normalized;
            rb.MovePosition(rb.position + Time.fixedDeltaTime * direction);
        }
    }
}
