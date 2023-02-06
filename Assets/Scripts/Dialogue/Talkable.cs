using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Talkable : MonoBehaviour
{
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private Sprite talkingSprite;
    [SerializeField] private float triggerDistance;
    [SerializeField] private bool doesStartWithPlayer;

    private string[] lines;
    private int currentLine;
    private TextMeshProUGUI text;
    private Image avatar;
    
    private int maxAmountOfSymbolsInLine = 180;
    private int currentSymbolInLine = 0;

    private void Start()
    {
        lines = dialogue.text.Split("\n"[0]);

        if (lines.Length == 0)
        {
            Debug.LogError("Character: " + gameObject.name + " has empty dialogue file");
        }
        
        currentLine = 0;
        text = Singleton.Instance.DialogueData.Text;
        avatar = Singleton.Instance.DialogueData.InterlocutarAvatar;
    }

    private void Update()
    {
        if (PauseMenuManager.IsPaused) return;
        
        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        if (Vector3.Distance(plPos, transform.position) > triggerDistance)
        {
            if (Singleton.Instance.DialogueData.IsActive)
            {
                HideDialogue();
            }
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!Singleton.Instance.DialogueData.IsActive)
            {
                DisplayDialogue();
            }
            
            if (TryDisplayCurrentLine())
            {
                currentLine++;
            }
        }
    }

    private bool TryDisplayCurrentLine()
    {
        if (currentLine >= lines.Length)
        {
            HideDialogue();
            return false;
        }

        if (doesStartWithPlayer)
        {
            if (currentLine % 2 == 0)
            {
                text.alignment = TextAlignmentOptions.TopRight;
            }
            else
            {
                text.alignment = TextAlignmentOptions.TopLeft;
            }
        }
        else
        {
            if (currentLine % 2 == 0)
            {
                text.alignment = TextAlignmentOptions.TopLeft;
            }
            else
            {
                text.alignment = TextAlignmentOptions.TopRight;
            }
        }
        

        string lineToOut = "";
        if (currentSymbolInLine + maxAmountOfSymbolsInLine < lines[currentLine].Length)
        {
            int latestSpace = 0;

            for (int i = currentSymbolInLine; i < currentSymbolInLine + maxAmountOfSymbolsInLine; i++)
            {
                if (lines[currentLine][i] != ' ') continue;

                latestSpace = i;
            }

            for (int i = currentSymbolInLine; i < latestSpace; i++)
            {
                lineToOut += lines[currentLine][i];
            }
            currentSymbolInLine = latestSpace + 1;
            text.text = lineToOut;
            return false;
        }
        
        for (int i = currentSymbolInLine; i < lines[currentLine].Length; i++)
        {
            lineToOut += lines[currentLine][i];
        }
        currentSymbolInLine = 0;
        text.text = lineToOut;
        avatar.sprite = talkingSprite;
        return true;
    }
    
    private void DisplayDialogue()
    {
        Singleton.Instance.DialogueData.DisplayDialogue();
    }

    private void HideDialogue()
    {
        Singleton.Instance.DialogueData.HideDialogue();
        currentLine = 0;
    }
}