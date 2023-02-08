using UnityEngine;

public class Recharge : MonoBehaviour
{
    [SerializeField] private float minLocalX;
    [SerializeField] private float maxLocalX;
    [SerializeField] private Transform value;

    private Weapon currentWeapon;

    private void Start()
    {
        WeaponChanged();
        Singleton.Instance.PlayerData.Player.GetComponent<PlayerWeaponHandler>().
            ChangedWeapon.AddListener(WeaponChanged);
        // This Event fixes bu_g where you see one frame when recharge line is not Flipped correctly
        PlayerAnimation.FlippedSide.AddListener(FlipSide);
    }

    private void Update()
    {
        float from0To1 = Mathf.Clamp(currentWeapon.GetElapsedTime() /
                                     currentWeapon.GetRechargeTime(), 0f, 1f);
        float x = minLocalX + from0To1 * (maxLocalX - minLocalX);
        value.localPosition = new Vector3(x, value.localPosition.y, 0f);
    }

    private void FlipSide()
    {
        float plScaleXSign = Mathf.Sign(Singleton.Instance.PlayerData.Player.transform.localScale.x);
        Vector3 scale = transform.localScale;
        if ((plScaleXSign < 0 && scale.x > 0) || (plScaleXSign > 0 && scale.x < 0))
        {
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }

    private void WeaponChanged()
    {
        currentWeapon = Singleton.Instance.PlayerData.Player.GetComponentInChildren<Weapon>();
    }
}
