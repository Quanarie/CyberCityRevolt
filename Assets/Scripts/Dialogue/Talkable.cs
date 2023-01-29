using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Talkable : MonoBehaviour
{
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private float triggerDistance;

    private string[] _lines;
    private int _currentLine;
    private GameObject _box;
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
        _box = Singleton.Instance.DialogueData.Box;
        _text = Singleton.Instance.DialogueData.Text;
        _plInput = Singleton.Instance.PlayerData.Input;
        PauseMenuManager.Paused.AddListener(HideWhilePause);
        PauseMenuManager.Resumed.AddListener(() => enabled = true);
    }

    private void Update()
    {
        var plPos = Singleton.Instance.PlayerData.Player.transform.position;
        if (Vector3.Distance(plPos, transform.position) > triggerDistance) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!_box.activeSelf)
            {
                DisplayDialogue();
            }
            
            if (TryDisplayCurrentLine())
            {
                _currentLine++;
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && _box.activeSelf)
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
        _box.SetActive(true);
    }

    private void HideDialogue()
    {
        _plInput.ActivateInput();
        _box.SetActive(false);
        _currentLine = 0;
    }

    private void HideWhilePause()
    {
        _box.SetActive(false);
        _currentLine = 0;
        enabled = false;
    }
}