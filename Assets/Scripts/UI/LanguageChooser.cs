using TMPro;
using UnityEngine;

public class LanguageChooser : MonoBehaviour
{
    private void Start()
    {
        if (!TryGetComponent(out TMP_Dropdown dropdown))
        {
            Debug.LogError("No Dropdown on languageChooser object");
        }
        dropdown.onValueChanged.AddListener(ChangeSavedLanguage);
        dropdown.value = PlayerPrefs.GetInt("Language");
    }

    private void ChangeSavedLanguage(int number)
    {
        PlayerPrefs.SetInt("Language", number);
    }
}
