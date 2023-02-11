using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Talkable : MonoBehaviour
{
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private Sprite talkingSprite;
    [SerializeField] private bool doesStartWithPlayer;

    private string[] lines;
    protected int currentLine;
    protected TextMeshProUGUI text;
    
    private float TIME_BETWEEN_LETTERS_SPAWN = 0.01f;
    private int MAX_AMOUNT_CHARACTERS_IN_LINE = 180;
    private int currentSymbolInLine = 0;
    protected bool isTyping = false;
    protected string lineToOut;
    protected bool isActive = false;

    private void Start()
    {
        lines = dialogue.text.Split("\n"[0]);

        if (lines.Length == 0)
        {
            Debug.LogError("Character: " + gameObject.name + " has empty dialogue file");
        }
        
        currentLine = 0;
        text = Singleton.Instance.DialogueData.Text;
    }

    protected bool TryDisplayCurrentLine()
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
        

        lineToOut = "";
        if (currentSymbolInLine + MAX_AMOUNT_CHARACTERS_IN_LINE < lines[currentLine].Length)
        {
            int latestSymbol = 0;

            for (int i = currentSymbolInLine; i < currentSymbolInLine + MAX_AMOUNT_CHARACTERS_IN_LINE; i++)
            {
                if (lines[currentLine][i] != ',' && lines[currentLine][i] != '.' ) continue;

                latestSymbol = i;
            }

            if (latestSymbol == 0)
            {
                for (int i = currentSymbolInLine; i < currentSymbolInLine + MAX_AMOUNT_CHARACTERS_IN_LINE; i++)
                {
                    if (lines[currentLine][i] != ' ' ) continue;

                    latestSymbol = i;
                }
            }

            for (int i = currentSymbolInLine; i <= latestSymbol; i++)
            {
                lineToOut += lines[currentLine][i];
            }
            currentSymbolInLine = latestSymbol + 1;
            StartCoroutine(DisplayString(lineToOut));
            return false;
        }
        
        for (int i = currentSymbolInLine; i < lines[currentLine].Length; i++)
        {
            lineToOut += lines[currentLine][i];
        }
        currentSymbolInLine = 0;
        StartCoroutine(DisplayString(lineToOut));
        return true;
    }

    private IEnumerator DisplayString(string line)
    {
        isTyping = true;
        text.text = "";
        for (int i = 0; i < line.Length; i++)
        {
            text.text += line[i];
            yield return new WaitForSeconds(TIME_BETWEEN_LETTERS_SPAWN);
        }
        isTyping = false;
    }

    protected void StopTyping()
    {
        StopAllCoroutines();
        isTyping = false;
    }
    
    protected void DisplayDialogue()
    {
        Singleton.Instance.StateManager.EnterDialogue();
        isActive = true;
        Singleton.Instance.DialogueData.InterlocutarAvatar.sprite = talkingSprite;
        Singleton.Instance.DialogueData.DisplayDialogue();
    }

    protected void HideDialogue()
    {
        Singleton.Instance.StateManager.LeaveDialogue();
        isActive = false;
        StopTyping();
        Singleton.Instance.DialogueData.HideDialogue();
        currentLine = 0;
        currentSymbolInLine = 0;
    }
}