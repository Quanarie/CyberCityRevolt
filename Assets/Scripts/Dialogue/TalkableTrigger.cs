using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableTrigger : Talkable
{
    private bool wasActivated = false;

    public bool IsActive() => isActive;
    
    protected override void Update()
    {
        if (PauseMenuManager.IsPaused || !isActive) return;

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

    public void Trigger()
    {
        DisplayDialogue();
        isActive = true;
            
        if (TryDisplayCurrentLine())
        {
            currentLine++;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<PlayerMovement>(out _)) return;

        if (!wasActivated)
        {
            Singleton.Instance.DialogueData.TalkableTriggers.Enqueue(this);
            wasActivated = true;
        }
    }
}