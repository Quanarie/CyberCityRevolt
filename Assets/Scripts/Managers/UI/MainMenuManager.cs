using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainContainer;
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button optionsBackButton;

    private void Start()
    {
        OpenMain();
        startButton.onClick.AddListener(() => StartGame());
        optionsButton.onClick.AddListener(() => OpenOptions());
        optionsBackButton.onClick.AddListener(() => OpenMain());
    }

    public void OpenOptions()
    {
        mainContainer.SetActive(false);
        optionsContainer.SetActive(true);
    }

    public void OpenMain()
    {
        mainContainer.SetActive(true);
        optionsContainer.SetActive(false);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
