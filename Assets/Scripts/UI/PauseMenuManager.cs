using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
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
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        Singleton.Instance.PlayerData.Input.DeactivateInput();
    }
    
    private void ClosePauseMenu()
    {
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
