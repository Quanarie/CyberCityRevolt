using TMPro;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [field : SerializeField] public GameObject Box { get; private set; }
    [field : SerializeField] public TextMeshProUGUI Text { get; private set; }
    
    public bool IsActive { get; private set; }

    private Animator animator;
    
    private static readonly int Opened = Animator.StringToHash("Opened");
    
    private void Start()
    {
        if (!Box.TryGetComponent(out animator))
        {
            Debug.LogError("No animator on DialogueBox");
        }
    }

    public void DisplayDialogue()
    {
        IsActive = true;
        animator.SetBool(Opened, true);
    }

    public void HideDialogue()
    {
        IsActive = false;
        animator.SetBool(Opened, false);
    }
}
