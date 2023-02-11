using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageObjects : MonoBehaviour, ISpawnable
{
    [SerializeField] private GameObject[] enemyObjects;
    [SerializeField] private float spawnRadius;

    private int amountOfSpawnedEnemies;
    private int amountOfDeadEnemies;

    private void Start()
    {
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemyObjects[i].SetActive(false);
        }
    }

    public void SpawnEnemies(Transform parent)
    {
        foreach (GameObject enemy in enemyObjects)
        {
            enemy.SetActive(true);
            enemy.transform.position = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            
            Singleton.Instance.EnemyManager.AddEnemyToLists(enemy);
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.Dying.AddListener(() => amountOfDeadEnemies++);
            
            amountOfSpawnedEnemies++;
        }
    }

    public bool IsDone() => amountOfSpawnedEnemies == amountOfDeadEnemies;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}