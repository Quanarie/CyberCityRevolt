using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] stages;
    [SerializeField] private Transform enemyParent;

    public static bool IsLevelComplete { get; private set; }
    
    private int currentStageNumber = 0;
    private ISpawnable currentStage;

    private void Start()
    {
        if (stages.Length == 0) return;
        
        stages[0].GetComponent<ISpawnable>().SpawnEnemies(enemyParent);
        currentStage = stages[0].GetComponent<ISpawnable>();
    }

    private void Update()
    {
        if (stages.Length == 0 || !currentStage.IsDone()) return;

        if (currentStageNumber < stages.Length - 1)
        {
            currentStageNumber++;
            currentStage = stages[currentStageNumber].GetComponent<ISpawnable>();
            currentStage.SpawnEnemies(enemyParent);
        }
        else
        {
            IsLevelComplete = true;
        }
    }
}
