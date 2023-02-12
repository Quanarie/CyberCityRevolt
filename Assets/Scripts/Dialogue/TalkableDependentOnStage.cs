using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableDependentOnStage : Talkable
{
    [SerializeField] private int stageShouldBeDone;
    private bool wasActivated = false;

    protected override void Start()
    {
        base.Start();
        Singleton.Instance.EnemySpawner.StageChanged.AddListener(TryActivate);
    }

    protected virtual void Update()
    {
        if (Singleton.Instance.StateManager.CurrentState == State.Paused || !isActive) return;

        if (Keyboard.current.qKey.wasPressedThisFrame && isActive)
        {
            HideDialogue();
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                StopTyping();
                text.text = lineToOut;
                return;
            }

            if (TryDisplayCurrentLine())
            {
                currentLine++;
            }
        }
    }

    private void TryActivate()
    {
        if (Singleton.Instance.DialogueData.IsActive || wasActivated ||
            !Singleton.Instance.EnemySpawner.IsStageDone(stageShouldBeDone)) return;
        
        Singleton.Instance.BulletSpawner.DestroyAllBullets();
        DisplayDialogue();
        if (TryDisplayCurrentLine())
        {
            currentLine++;
        }
        wasActivated = true;
    }

}