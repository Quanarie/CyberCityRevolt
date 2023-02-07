using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableTrigger : Talkable
{
    private bool wasActivated = false;
    
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<PlayerMovement>(out _)) return;
        
        if (PauseMenuManager.IsPaused) return;
        
        if (!Singleton.Instance.DialogueData.IsActive && !wasActivated)
        {
            DisplayDialogue();
            wasActivated = true;
            isActive = true;
            
            if (TryDisplayCurrentLine())
            {
                currentLine++;
            }
        }
    }
}