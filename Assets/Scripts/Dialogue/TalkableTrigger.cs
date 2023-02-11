using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableTrigger : Talkable
{
    private bool wasActivated = false;

    private void Update()
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<PlayerMovement>(out _)) return;

        if (!wasActivated)
        {
            DisplayDialogue();
            
            if (TryDisplayCurrentLine())
            {
                currentLine++;
            }
            wasActivated = true;
        }
    }
}