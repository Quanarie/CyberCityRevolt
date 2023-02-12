using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableNpc : Talkable
{
    [SerializeField] protected float triggerDistance;
    
    private void Update()
    {
        if (Singleton.Instance.StateManager.CurrentState == State.Paused ||
            Singleton.Instance.NpcManager.FindClosestNpcToPlayer() != this) return;

        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        if (Vector3.Distance(plPos, transform.position) > triggerDistance) return;
        
        if (Keyboard.current.qKey.wasPressedThisFrame && isActive)
        {
            HideDialogue();
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!isActive)
            {
                if (Singleton.Instance.DialogueData.IsActive) return;
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