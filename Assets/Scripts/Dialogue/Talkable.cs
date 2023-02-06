using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Talkable : MonoBehaviour
{
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private float triggerDistance;

    private string[] _lines;
    private int _currentLine;
    private TextMeshProUGUI _text;
    private PlayerInput _plInput;

    private void Start()
    {
        _lines = dialogue.text.Split("\n"[0]);

        if (_lines.Length == 0)
        {
            Debug.LogError("Character: " + gameObject.name + " has empty dialogue file");
        }
        
        _currentLine = 0;
        _text = Singleton.Instance.DialogueData.Text;
        _plInput = Singleton.Instance.PlayerData.Input;
    }

    private void Update()
    {
        if (PauseMenuManager.IsPaused) return;
        
        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        if (Vector3.Distance(plPos, transform.position) > triggerDistance) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!Singleton.Instance.DialogueData.IsActive)
            {
                DisplayDialogue();
            }
            
            if (TryDisplayCurrentLine())
            {
                _currentLine++;
            }
        }

        if (Keyboard.current.qKey.wasPressedThisFrame && Singleton.Instance.DialogueData.IsActive)
        {
            HideDialogue();
        }
    }

    private bool TryDisplayCurrentLine()
    {
        if (_currentLine < _lines.Length)
        {
            _text.text = _lines[_currentLine];
            return true;
        }
        
        HideDialogue();
        return false;
    }
    
    private void DisplayDialogue()
    {
        _plInput.DeactivateInput();
        Singleton.Instance.DialogueData.DisplayDialogue();
    }

    private void HideDialogue()
    {
        _plInput.ActivateInput();
        Singleton.Instance.DialogueData.HideDialogue();
        _currentLine = 0;
    }
}