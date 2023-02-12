using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent EnemyDied;
    [HideInInspector] public List<EnemyMovement> EnemiesSpawnedMovements;
    
    private void Awake()
    {
        EnemiesSpawnedMovements = new();
        foreach (EnemyMovement enemy in FindObjectsByType<EnemyMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            AddEnemyToLists(enemy.gameObject);
        }
    }
    
    public void AddEnemyToLists(GameObject enemy)
    {
        if (!enemy.TryGetComponent(out EnemyHealth enemyHealth))
        {
            Debug.LogError("No health component on: " + enemy.name);
        }
        
        if (!enemy.TryGetComponent(out EnemyMovement enemyMovement))
        {
            Debug.LogError("No movement component on: " + enemy.name);
        }
        
        EnemiesSpawnedMovements.Add(enemyMovement);
        enemyHealth.Dying.AddListener(() => RemoveEnemyFromLists(enemy));
    }

    public void RemoveEnemyFromLists(GameObject enemy)
    {
        if (!enemy.TryGetComponent(out EnemyMovement enemyMovement))
        {
            Debug.LogError("No movement component on: " + enemy.name);
        }
        
        EnemiesSpawnedMovements.Remove(enemyMovement);
        EnemyDied?.Invoke();
    }
}