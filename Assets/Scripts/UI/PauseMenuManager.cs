using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public static readonly UnityEvent Paused = new();
    public static readonly UnityEvent Resumed = new();
    
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private Button mainMenuButton;
    
    private void Start()
    {
        pauseMenuObject.SetActive(false);
        mainMenuButton.onClick.AddListener(() => LoadMainMenu());
    }
    
    private void Update()
    {
        if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;

        if (pauseMenuObject.activeSelf)
        {
            ClosePauseMenu();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        Paused?.Invoke();
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        Singleton.Instance.PlayerData.Input.DeactivateInput();
    }
    
    private void ClosePauseMenu()
    {
        Resumed?.Invoke();
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        Singleton.Instance.PlayerData.Input.ActivateInput();
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
