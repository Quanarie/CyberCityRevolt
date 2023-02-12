using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueData : MonoBehaviour
{
    [field : SerializeField] public GameObject Box { get; private set; }
    [field : SerializeField] public TextMeshProUGUI Text { get; private set; }
    [field : SerializeField] public Image InterlocutarAvatar { get; private set; }
    [field : SerializeField] public Image PlayerAvatar { get; private set; }
    
    public bool IsActive { get; private set; }

    private Animator boxAnimator;
    private Animator interlocutarAvatarAnimator;
    private Animator playerAvatarAnimator;
    private AnimationClip boxCloseAnimation;

    
    private static readonly int Opened = Animator.StringToHash("Opened");
    
    private void Start()
    {
        if (!Box.TryGetComponent(out boxAnimator))
        {
            Debug.LogError("No animator on DialogueBox");
        }
        
        if (!InterlocutarAvatar.TryGetComponent(out interlocutarAvatarAnimator))
        {
            Debug.LogError("No animator on InterlocutarDialogueAvatar");
        }
        
        if (!PlayerAvatar.TryGetComponent(out playerAvatarAnimator))
        {
            Debug.LogError("No animator on PlayerDialogueAvatar");
        }

        boxCloseAnimation = PlayerAnimation.FindAnimation(boxAnimator, "Close");
    }

    public void DisplayDialogue()
    {
        IsActive = true;
        boxAnimator.SetBool(Opened, true);
        interlocutarAvatarAnimator.SetBool(Opened, true);
        playerAvatarAnimator.SetBool(Opened, true);
    }

    public void HideDialogue()
    {
        StartCoroutine(SetInactiveInTheEndOfAnimation(boxCloseAnimation.length));
        boxAnimator.SetBool(Opened, false);
        interlocutarAvatarAnimator.SetBool(Opened, false);
        playerAvatarAnimator.SetBool(Opened, false);
    }

    private IEnumerator SetInactiveInTheEndOfAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        IsActive = false;
    }
}
