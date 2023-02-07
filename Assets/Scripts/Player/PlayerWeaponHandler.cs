using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    [HideInInspector] public UnityEvent ChangedWeapon;

    [SerializeField] private Vector2 weaponOffset;

    private Weapon currentWeapon;
    private static List<Weapon> weaponsOnFloor = new ();

    public static void StoreWeapon(Weapon toStore)
    {
        if (weaponsOnFloor.Contains(toStore)) return;
        weaponsOnFloor.Add(toStore);
    }
    public static void RemoveWeapon(Weapon toRemove) => weaponsOnFloor.Remove(toRemove);
    
    private const float PICKUP_DISTANCE = 0.5f;

    private void Start()
    {
        currentWeapon = GetComponentInChildren<Weapon>();
        currentWeapon.transform.localPosition = weaponOffset;
    }
    
    private void Update()
    {
        Weapon closestWeapon = GetClosestWeapon();
        if (closestWeapon == null)
        {
            Singleton.Instance.WeaponInfoManager.HideInfo();
            return;
        }
        
        Singleton.Instance.WeaponInfoManager.ShowInfo(closestWeapon);
    }

    public void OnPickup(InputValue value)
    {
        Weapon closestWeapon = GetClosestWeapon();
        if (closestWeapon == null) return;
        
        currentWeapon.DropWeapon();
        closestWeapon.PickupWeapon(transform);
        closestWeapon.transform.localPosition = weaponOffset;
        currentWeapon = closestWeapon;
        ChangedWeapon?.Invoke();
    }
    
    private Weapon GetClosestWeapon()
    {
        if (weaponsOnFloor.Count == 0) return null;

        Vector3 myPos = transform.position;
        Weapon closestWeapon = weaponsOnFloor[0];
        for (int i = 1; i < weaponsOnFloor.Count; i++)
        {
            if (Vector3.Distance(myPos, weaponsOnFloor[i].transform.position) <
                Vector3.Distance(myPos, closestWeapon.transform.position))
            {
                closestWeapon = weaponsOnFloor[i];
            }
        }
        
        if (Vector3.Distance(myPos, closestWeapon.transform.position) > PICKUP_DISTANCE)
        {
            return null;
        }
        
        return closestWeapon;
    }
} 
