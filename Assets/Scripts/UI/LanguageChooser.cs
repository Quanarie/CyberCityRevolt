using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageChooser : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    
    private void Start()
    {
        dropdown.onValueChanged.AddListener(ChangeSavedLanguage);
        dropdown.value = PlayerPrefs.GetInt("Language");
        StartCoroutine(SetLocale(dropdown.value));
    }

    private void ChangeSavedLanguage(int number)
    {
        PlayerPrefs.SetInt("Language", number);
        StartCoroutine(SetLocale(number));
    }

    private IEnumerator SetLocale(int id)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
    }
}
