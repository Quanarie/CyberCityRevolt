using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private Button mainMenuButton;

    // This is made so pause menu does not turn off input if it was turned off
    // (e.g. when player calls pause menu while in dialogie)
    private bool wasInputActive;
    
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
        IsPaused = true;
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        wasInputActive = Singleton.Instance.PlayerData.Input.inputIsActive;
        Singleton.Instance.PlayerData.Input.DeactivateInput();
    }
    
    private void ClosePauseMenu()
    {
        IsPaused = false;
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        if (wasInputActive)
        {
            Singleton.Instance.PlayerData.Input.ActivateInput();
        }
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
