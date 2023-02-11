using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector] public List<EnemyMovement> EnemiesSpawnedMovements = new();

    private void Start()
    {
        foreach (EnemyMovement enemy in FindObjectsByType<EnemyMovement>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            AddEnemyToLists(enemy.gameObject);
        }
    }

    public void AddEnemyToLists(GameObject enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        EnemiesSpawnedMovements.Add(enemyMovement);
        enemyHealth.Dying.AddListener(() => EnemiesSpawnedMovements.Remove(enemyMovement));
    }
}