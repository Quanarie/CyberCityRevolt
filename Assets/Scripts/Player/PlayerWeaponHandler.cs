using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private Vector2 weaponOffset;
    
    private const float PICKUP_DISTANCE = 0.5f;

    public void OnPickup(InputValue value)
    {
        var weaponsOnFloor = Physics2D.OverlapCircleAll(transform.position,
            PICKUP_DISTANCE, LayerMask.GetMask("Weapon"));

        if (weaponsOnFloor.Length <= 1) return;
        
        var currentWeapon = GetComponentInChildren<Weapon>();
        currentWeapon.DropWeapon();

        weaponsOnFloor[1].transform.SetParent(Singleton.Instance.PlayerData.Player.transform);
        weaponsOnFloor[1].transform.localPosition = weaponOffset;
        var weaponComponent = weaponsOnFloor[1].GetComponent<Weapon>();
        weaponComponent.PickupWeapon(transform);
    }
}
