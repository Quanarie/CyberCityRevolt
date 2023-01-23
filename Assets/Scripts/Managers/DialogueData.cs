using TMPro;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [field : SerializeField] public GameObject Box { get; private set; }
    [field : SerializeField] public TextMeshProUGUI Text { get; private set; }

    private void Start()
    {
        Box.SetActive(false);
    }
}
