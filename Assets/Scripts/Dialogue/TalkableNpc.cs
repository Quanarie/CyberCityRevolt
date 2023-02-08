using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableNpc : Talkable
{
    [SerializeField] protected float triggerDistance;
    
    private void Update()
    {
        if (PauseMenuManager.IsPaused) return;

        if (Singleton.Instance.DialogueData.IsActive && !isActive) return;

        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        if (Vector3.Distance(plPos, transform.position) > triggerDistance)
        {
            if (isActive)
            {
                HideDialogue();
            }
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!isActive)
            {
                DisplayDialogue();
            }

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
}