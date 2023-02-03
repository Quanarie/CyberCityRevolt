using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyInfo))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToOtherEnemies;
    [SerializeField] private Transform route;
    [SerializeField] private float waitOnPointTime;
    
    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private EnemyInfo _info;
    private Vector3[] _routePoints;
    private int _currentRoutePoint = 0;
    private const float MINIMAL_DISTANCE_TO_POINT = 0.1f;
    private bool _isWaiting = false;

    private void Start()
    {
        if (!TryGetComponent(out _rb))
        {
            Debug.LogError("No Rigidbody2D on Enemy" + gameObject.name);
        }

        _playerTransform = Singleton.Instance.PlayerData.Player.transform;
        _info = GetComponent<EnemyInfo>();

        if (route == null || route.childCount <= 1)
        {
            _routePoints = new[] { transform.position };
            return;
        }
        
        _routePoints = new Vector3[route.childCount];
        for (int i = 0; i < route.childCount; i++)
        {
            _routePoints[i] = route.GetChild(i).position;
        }
    }

    private void FixedUpdate()
    {
        CollideWithOtherEnemies();

        Vector3 plPos = _playerTransform.position;

        float distanceToPlayer = Vector3.Distance(plPos, transform.position);

        if (distanceToPlayer > _info.TriggerDistance)
        {
            MoveOnRoute();
        }
        else if (distanceToPlayer > _info.MinDistanceToPlayer)
        {
            MoveTo(plPos);
        }
        else
        {
            _info.MoveDirection = Vector2.zero;
        }
    }
    
    private void MoveOnRoute()
    {
        Vector3 moveToPoint = _routePoints[_currentRoutePoint];
        if (Vector3.Distance(moveToPoint, transform.position) < MINIMAL_DISTANCE_TO_POINT)
        {
            if (_isWaiting) return;
            StartCoroutine(WaitAndSetNextPoint());
            return;
        }
        
        MoveTo(moveToPoint);
    }
    
    private void MoveTo(Vector3 destination)
    {
        _info.MoveDirection = destination - transform.position;
        _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * _info.MoveDirection.normalized);
    }

    IEnumerator WaitAndSetNextPoint()
    {
        _isWaiting = true;
        _info.MoveDirection = Vector2.zero;
        yield return new WaitForSeconds(waitOnPointTime);
        _isWaiting = false;
        SetNextRoutePoint();
    }

    private void SetNextRoutePoint()
    {
        if (_currentRoutePoint == _routePoints.Length - 1)
        {
            _currentRoutePoint = 0;
            return;
        }
        _currentRoutePoint++;
    }

    private void CollideWithOtherEnemies()
    {
        foreach (var col in Physics2D.OverlapCircleAll(transform.position, minDistanceToOtherEnemies))
        {
            if (!col.TryGetComponent<EnemyInfo>(out _) || col.gameObject == gameObject) continue;

            Vector3 enPos = col.transform.position;
            Vector3 myPos = transform.position;
            Vector2 direction = new Vector2(myPos.x - enPos.x, myPos.y - enPos.y).normalized;
            _rb.MovePosition(_rb.position + Time.fixedDeltaTime * direction);
        }
    }
}
