using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] stagesContainers;

    private int currentStageNumber = 0;
    private int amountOfSpawnedEnemiesCurrStage = 0;
    private int amountOfDeadEnemiesCurrStage = 0;

    private void Start()
    {
        foreach (GameObject stage in stagesContainers)
        {
            for (int i = 0; i < stage.transform.childCount; i++)
            {
                stage.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        SpawnEnemies(stagesContainers[0]);
    }

    private void Update()
    {
        if (amountOfSpawnedEnemiesCurrStage != amountOfDeadEnemiesCurrStage || 
            currentStageNumber == stagesContainers.Length - 1) return;
        
        currentStageNumber++;
        SpawnEnemies(stagesContainers[currentStageNumber]);
    }
    
    public void SpawnEnemies(GameObject stageContainer)
    {
        amountOfDeadEnemiesCurrStage = 0;
        amountOfSpawnedEnemiesCurrStage = 0;
        for (int i = 0; i < stageContainer.transform.childCount; i++)
        {
            GameObject enemy = stageContainer.transform.GetChild(i).gameObject;
            enemy.SetActive(true);
            Singleton.Instance.EnemyManager.AddEnemyToLists(enemy);
            if (!enemy.TryGetComponent(out EnemyHealth enemyHealth))
            {
                Debug.LogError("No health component on: " + enemy.name);
            }
            enemyHealth.Dying.AddListener(() => amountOfDeadEnemiesCurrStage++);
            amountOfSpawnedEnemiesCurrStage++;
        }
    }
}
