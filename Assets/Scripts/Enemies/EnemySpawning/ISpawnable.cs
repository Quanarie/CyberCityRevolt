using UnityEngine;

public interface ISpawnable
{
    public void SpawnEnemies(Transform parent);
    public bool IsDone();
}