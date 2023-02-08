using UnityEngine;

public class StagePrefabs : MonoBehaviour, ISpawnable
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int[] amountOfEveryEnemy;
    [SerializeField] private float spawnRadius;

    private int amountOfSpawnedEnemies;
    private int amountOfDeadEnemies;

    public void SpawnEnemies(Transform parent)
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            for (int j = 0; j < amountOfEveryEnemy[i]; j++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i], parent);
                enemy.transform.position = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
                
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                EnemySpawner.EnemiesSpawned.Add(enemyMovement);
                enemyHealth.Dying.AddListener(() => EnemySpawner.EnemiesSpawned.Remove(enemyMovement));
                enemyHealth.Dying.AddListener(() => amountOfDeadEnemies++);
                amountOfSpawnedEnemies++;
            }
        }
    }

    public bool IsDone() => amountOfSpawnedEnemies == amountOfDeadEnemies;
}