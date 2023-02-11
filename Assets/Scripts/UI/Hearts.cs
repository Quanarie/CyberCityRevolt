using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    [SerializeField] private GameObject fullHeartPrefab;
    [SerializeField] private GameObject halfHeartPrefab;
    [SerializeField] private float distanceBetweenHearts;

    private List<GameObject> hearts;
    
    private void Start()
    {
        hearts = new();
        Singleton.Instance.PlayerData.Health.ChangedHp.AddListener(ChangeHearts);
        SpawnHearts(Singleton.Instance.PlayerData.Health.GetMaxHp());
    }

    private void ChangeHearts(int currentHp)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            Destroy(hearts[i]);
        }
        SpawnHearts(currentHp);
    }

    private void SpawnHearts(int currentHp)
    {
        float offsetX = 0f;
        for (int i = 0; i < currentHp / 2; i++)
        {
            SpawnHeart(fullHeartPrefab, offsetX);
            offsetX += distanceBetweenHearts;
        }

        if (currentHp % 2 == 1)
        {
            SpawnHeart(halfHeartPrefab, offsetX);
        }
    }

    private void SpawnHeart(GameObject prefab, float offsetX)
    {
        GameObject heart = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        heart.GetComponent<RectTransform>().localPosition = new Vector3(offsetX, 0f, 0f);
        hearts.Add(heart);
    }
}
