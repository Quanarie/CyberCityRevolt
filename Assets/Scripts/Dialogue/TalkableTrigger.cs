using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableTrigger : Talkable
{
    private bool wasActivated = false;

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

    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        if (!col.TryGetComponent<PlayerMovement>(out _) || wasActivated ||
            Singleton.Instance.DialogueData.IsActive) return;

        Singleton.Instance.BulletManager.DestroyAllBullets();
        DisplayDialogue();
            
        if (TryDisplayCurrentLine())
        {
            currentLine++;
        }
        wasActivated = true;
    }
}