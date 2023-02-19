using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemySpawnedParticles;
    [HideInInspector] public UnityEvent StageChanged;
    [HideInInspector] public List<GameObject> EnemiesActive;
    
    private GameObject[] stagesContainers;

    private int currentStageNumber = 0;
    private int amountOfSpawnedEnemiesCurrStage = 0;
    private int amountOfDeadEnemiesCurrStage = 0;

    private void Start()
    {
        stagesContainers = GameObject.FindGameObjectsWithTag("Stage");
        
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
        if (currentStageNumber == stagesContainers.Length) return;
        if (amountOfSpawnedEnemiesCurrStage != amountOfDeadEnemiesCurrStage) return;

        if (currentStageNumber == stagesContainers.Length - 1)
        {
            currentStageNumber++;
            StageChanged?.Invoke();
            return;
        }
        
        currentStageNumber++;
        SpawnEnemies(stagesContainers[currentStageNumber]);
        StageChanged?.Invoke();
    }
    
    private void SpawnEnemies(GameObject stageContainer)
    {
        amountOfDeadEnemiesCurrStage = 0;
        amountOfSpawnedEnemiesCurrStage = 0;
        for (int i = 0; i < stageContainer.transform.childCount; i++)
        {
            GameObject enemy = stageContainer.transform.GetChild(i).gameObject;
            enemy.SetActive(true);
            Instantiate(enemySpawnedParticles, enemy.transform.position, enemy.transform.rotation);
            if (!enemy.TryGetComponent(out EnemyHealth enemyHealth))
            {
                Debug.LogError("No health component on: " + enemy.name);
            }
            
            EnemiesActive.Add(enemy);
            enemyHealth.Dying.AddListener(() => EnemiesActive.Remove(enemy));

            enemyHealth.Dying.AddListener(() => amountOfDeadEnemiesCurrStage++);
            amountOfSpawnedEnemiesCurrStage++;
        }
    }

    public bool IsStageDone(int stageIndex) => stageIndex < currentStageNumber;
}
