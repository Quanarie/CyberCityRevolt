using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueData : MonoBehaviour
{
    [field : SerializeField] public GameObject Box { get; private set; }
    [field : SerializeField] public TextMeshProUGUI Text { get; private set; }
    [field : SerializeField] public Image Avatar { get; private set; }
    
    public bool IsActive { get; private set; }

    private Animator boxAnimator;
    private Animator avatarAnimator;
    
    private static readonly int Opened = Animator.StringToHash("Opened");
    
    private void Start()
    {
        if (!Box.TryGetComponent(out boxAnimator))
        {
            Debug.LogError("No animator on DialogueBox");
        }
        
        if (!Avatar.TryGetComponent(out avatarAnimator))
        {
            Debug.LogError("No animator on DialogueAvatar");
        }
    }

    public void DisplayDialogue()
    {
        IsActive = true;
        boxAnimator.SetBool(Opened, true);
        avatarAnimator.SetBool(Opened, true);
    }

    public void HideDialogue()
    {
        IsActive = false;
        boxAnimator.SetBool(Opened, false);
        avatarAnimator.SetBool(Opened, false);
    }
}
