using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private EnemyInfo info;
    
    [SerializeField] private Transform route;
    [SerializeField] private float minWaitOnPointTime;
    [SerializeField] private float maxWaitOnPointTime;
    private Vector3[] routePoints;
    private int currentRoutePoint = 0;
    private const float MINIMAL_DISTANCE_TO_POINT = 0.1f;
    private bool isWaiting = false;

    [SerializeField] private float timeToChangeEnemyToGoAwayFrom;
    private GameObject currentEnemyToGoAwayFrom;
    private float elapsedSinceEnemyToGoAwayFromChange = 0f;

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

        elapsedSinceEnemyToGoAwayFromChange += Time.fixedDeltaTime;
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
        if (elapsedSinceEnemyToGoAwayFromChange > timeToChangeEnemyToGoAwayFrom)
        {
            currentEnemyToGoAwayFrom = GetClosestEnemy();
            elapsedSinceEnemyToGoAwayFromChange = 0f;
        }
        
        Vector2 toDest = (destination - transform.position).normalized;
        if (currentEnemyToGoAwayFrom == null)
        {
            info.MoveDirection = toDest;
        }
        else
        {
            Vector2 fromEnemy = (transform.position - currentEnemyToGoAwayFrom.transform.position).normalized;
            fromEnemy /= Vector3.Distance(transform.position, currentEnemyToGoAwayFrom.transform.position);
            info.MoveDirection = toDest + fromEnemy;
        }
        
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * info.MoveDirection.normalized);
    }
    
    public GameObject GetClosestEnemy()
    {
        List<GameObject> enemies = Singleton.Instance.EnemySpawner.EnemiesActive;
        if (enemies.Count <= 1) return null;
        
        GameObject closestEnemy = null;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == gameObject) continue;

            if (closestEnemy == null)
            {
                closestEnemy = enemies[i];
            }
            else if (Vector3.Distance(transform.position, enemies[i].transform.position) < 
                     Vector3.Distance(transform.position, closestEnemy.transform.position))
            {
                closestEnemy = enemies[i];
            }
        }

        return closestEnemy;
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
