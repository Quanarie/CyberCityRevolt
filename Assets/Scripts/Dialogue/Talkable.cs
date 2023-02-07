using System.Collections;
using System.Collections.Generic;
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
    
    private float TIME_BETWEEN_LETTERS_SPAWN = 0.01f;
    private int MAX_AMOUNT_CHARACTERS_IN_LINE = 180;
    private int currentSymbolInLine = 0;
    private bool isTyping = false;
    private string lineToOut;

    private static List<Talkable> talkableCharacters = new();

    private void Start()
    {
        lines = dialogue.text.Split("\n"[0]);

        if (lines.Length == 0)
        {
            Debug.LogError("Character: " + gameObject.name + " has empty dialogue file");
        }
        
        currentLine = 0;
        text = Singleton.Instance.DialogueData.Text;
        talkableCharacters.Add(this);
    }

    private static Talkable FindClosestTalkableToPlayer()
    {
        Talkable closestTalkable = talkableCharacters[0];
        Vector3 plPos = Singleton.Instance.PlayerData.Player.transform.position;
        for (int i = 1; i < talkableCharacters.Count; i++)
        {
            if (Vector3.Distance(plPos, talkableCharacters[i].transform.position) <
                Vector3.Distance(plPos, closestTalkable.transform.position))
            {
                closestTalkable = talkableCharacters[i];
            }
        }

        return closestTalkable;
    }

    private void Update()
    {
        if (PauseMenuManager.IsPaused) return;

        if (FindClosestTalkableToPlayer() != this) return;
        
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
        

        lineToOut = "";
        if (currentSymbolInLine + MAX_AMOUNT_CHARACTERS_IN_LINE < lines[currentLine].Length)
        {
            int latestSpace = 0;

            for (int i = currentSymbolInLine; i < currentSymbolInLine + MAX_AMOUNT_CHARACTERS_IN_LINE; i++)
            {
                if (lines[currentLine][i] != ' ') continue;

                latestSpace = i;
            }

            for (int i = currentSymbolInLine; i < latestSpace; i++)
            {
                lineToOut += lines[currentLine][i];
            }
            currentSymbolInLine = latestSpace + 1;
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

    private void StopTyping()
    {
        StopAllCoroutines();
        isTyping = false;
    }
    
    private void DisplayDialogue()
    {
        Singleton.Instance.DialogueData.InterlocutarAvatar.sprite = talkingSprite;
        Singleton.Instance.DialogueData.DisplayDialogue();
    }

    private void HideDialogue()
    {
        StopTyping();
        Singleton.Instance.DialogueData.HideDialogue();
        currentLine = 0;
        currentSymbolInLine = 0;
    }
}