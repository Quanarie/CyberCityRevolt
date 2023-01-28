using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    [HideInInspector] public UnityEvent ChangedWeapon;

    [SerializeField] private Vector2 weaponOffset;
    
    private const float PICKUP_DISTANCE = 0.5f;
    
    public void OnPickup(InputValue value)
    {
        var weaponsOnFloor = Physics2D.OverlapCircleAll(transform.position,
            PICKUP_DISTANCE, LayerMask.GetMask("Weapon"));

        if (weaponsOnFloor.Length <= 1) return;

        var currentWeapon = GetComponentInChildren<Weapon>();
        currentWeapon.DropWeapon();

        SortWeaponsByDistance(ref weaponsOnFloor);

        Collider2D closestWeapon = null;
        if (weaponsOnFloor[0].gameObject == currentWeapon.gameObject)
        {
            closestWeapon = weaponsOnFloor[1];
        }
        else
        {
            closestWeapon = weaponsOnFloor[0];
        }

        closestWeapon.transform.SetParent(Singleton.Instance.PlayerData.Player.transform);
        closestWeapon.transform.localPosition = weaponOffset;
        var weaponComponent = closestWeapon.GetComponent<Weapon>();
        weaponComponent.PickupWeapon(transform);
        ChangedWeapon?.Invoke();
    }
    
    private void SortWeaponsByDistance(ref Collider2D[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            if (Vector3.Distance(transform.position, arr[i].transform.position) <
                Vector3.Distance(transform.position, arr[i + 1].transform.position)) continue;

            Collider2D temp = arr[i];
            arr[i] = arr[i + 1];
            arr[i + 1] = temp;
            i = 0;
        }
    }
} 
