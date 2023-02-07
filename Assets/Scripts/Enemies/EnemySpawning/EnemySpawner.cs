using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Stage[] stages;
    [SerializeField] private Transform enemyParent;

    public static List<Transform> EnemiesSpawned = new();
    public static bool IsLevelComplete { get; private set; }
    
    private int currentStage = 0;

    private void Start()
    {
        if (stages.Length == 0) return;
        
        stages[0].SpawnEnemies(enemyParent);
    }

    private void Update()
    {
        if (!stages[currentStage].IsDone()) return;

        if (currentStage < stages.Length - 1)
        {
            currentStage++;
            stages[currentStage].SpawnEnemies(enemyParent);
        }
        else
        {
            IsLevelComplete = true;
        }
    }
}
