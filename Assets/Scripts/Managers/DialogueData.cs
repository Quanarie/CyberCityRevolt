using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueData : MonoBehaviour
{
    [field : SerializeField] public CanvasGroup Wrapper { get; private set; }
    [field : SerializeField] public GameObject Box { get; private set; }
    [field : SerializeField] public TextMeshProUGUI Text { get; private set; }
    [field : SerializeField] public Image InterlocutarAvatar { get; private set; }
    [field : SerializeField] public Image PlayerAvatar { get; private set; }
    
    public Queue<TalkableTrigger> TalkableTriggers { get; set; } = new();
    public bool IsActive { get; private set; }

    private Animator boxAnimator;
    private Animator interlocutarAvatarAnimator;
    private Animator playerAvatarAnimator;
    private AnimationClip boxCloseAnimation;
    
    private static readonly int Opened = Animator.StringToHash("Opened");
    
    private const float DIALOGUE_ALPHA_WHILE_FIGHTING = 0.4f;
    
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

    private void Update()
    {
        if (isThereTriggeredEnemy())
        {
            Wrapper.alpha = DIALOGUE_ALPHA_WHILE_FIGHTING;
        }
        else
        {
            Wrapper.alpha = 1f;
        }
        
        if (TalkableTriggers.Count == 0 || IsActive) return;
        
        TalkableTriggers.Dequeue().Trigger();
    }

    private bool isThereTriggeredEnemy()
    {
        foreach (EnemyMovement enemy in EnemySpawner.EnemiesSpawned)
        {
            if (enemy.IsTriggered()) return true;
        }

        return false;
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
