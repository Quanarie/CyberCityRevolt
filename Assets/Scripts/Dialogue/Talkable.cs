using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Talkable : MonoBehaviour
{
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private float triggerDistance;

    private string[] lines;
    private int currentLine;
    private GameObject box;
    private TextMeshProUGUI text;
    private PlayerInput plInput;

    private void Start()
    {
        lines = dialogue.text.Split("\n"[0]);

        if (lines.Length == 0)
        {
            Debug.LogError("Character: " + gameObject.name + " has empty dialogue file");
        }
        
        currentLine = 0;
        box = Singleton.Instance.DialogueData.Box;
        text = Singleton.Instance.DialogueData.Text;
        plInput = Singleton.Instance.PlayerData.Input;
    }

    private void Update()
    {
        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        if (Vector3.Distance(plPos, transform.position) > triggerDistance) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame && box.activeSelf)
        {
            HideDialogue();
        }

        if (!Keyboard.current.spaceKey.wasPressedThisFrame) return;

        if (!box.activeSelf) DisplayDialogue();

        if (TryDisplayCurrentLine()) currentLine++;
    }

    private bool TryDisplayCurrentLine()
    {
        if (currentLine < lines.Length)
        {
            text.text = lines[currentLine];
            return true;
        }
        else
        {
            HideDialogue();
        }
        return false;
    }
    
    private void DisplayDialogue()
    {
        plInput.DeactivateInput();
        box.SetActive(true);
    }

    private void HideDialogue()
    {
        plInput.ActivateInput();
        box.SetActive(false);
        currentLine = 0;
    }
}