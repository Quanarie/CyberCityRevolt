using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public Language Language = Language.En;

    private void Start()
    {
        Language = (Language)PlayerPrefs.GetInt("Language");
    }
}

public enum Language
{
    En,
    Uk
}