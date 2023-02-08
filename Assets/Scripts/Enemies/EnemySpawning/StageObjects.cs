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
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            EnemySpawner.EnemiesSpawned.Add(enemyMovement);
            enemyHealth.Dying.AddListener(() => EnemySpawner.EnemiesSpawned.Remove(enemyMovement));
            enemyHealth.Dying.AddListener(() => amountOfDeadEnemies++);
            
            amountOfSpawnedEnemies++;
        }
    }

    public bool IsDone() => amountOfSpawnedEnemies == amountOfDeadEnemies;
}