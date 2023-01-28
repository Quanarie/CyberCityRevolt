using TMPro;
using UnityEngine;

public class CrosshairChooser : MonoBehaviour
{
    private void Start()
    {
        if (!TryGetComponent(out TMP_Dropdown dropdown))
        {
            Debug.LogError("No Dropdown on crossHairChooser object");
        }
        dropdown.onValueChanged.AddListener(ChangeSavedCrosshair);
        dropdown.value = PlayerPrefs.GetInt("Crosshair");
    }

    private void ChangeSavedCrosshair(int number)
    {
        PlayerPrefs.SetInt("Crosshair", number);
    }
}
