using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoManager : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private Image image;
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI rechargeTime;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI bulletSpeed;
    [SerializeField] private TextMeshProUGUI amountOfBullets;

    private void Start()
    {
        HideInfo();
    }

    public void ShowInfo(Weapon weapon)
    {
        window.SetActive(true);

        if (!weapon.gameObject.TryGetComponent(out SpriteRenderer weaponSpriteRenderer))
        {
            Debug.LogError("No spriteRenderer on: " + weapon.gameObject.name);
        }

        WeaponInfo info = weapon.GetWeaponInfo();
        
        image.sprite = weaponSpriteRenderer.sprite;
        name.text = info.name;
        rechargeTime.text = info.rechargeTime.ToString();
        damage.text = info.damage.ToString();
        bulletSpeed.text = info.bulletSpeed.ToString();
        amountOfBullets.text = info.amountOfBullets.ToString();
    }

    public void HideInfo()
    {
        window.SetActive(false);
    }
}
