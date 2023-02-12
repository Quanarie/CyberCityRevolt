using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponInput : MonoBehaviour
{
    /// <summary>
    /// Invoke Shoot when enemy shoudl try to shoot
    /// </summary>
    [HideInInspector] public UnityEvent<Vector2> Shoot;

    [field : SerializeField] public Vector2 WeaponOffset { get; private set; }

    /// <returns> Place where weapon should be pointing </returns>
    public abstract Vector2 GetWhereToAim();

    protected virtual void Start()
    {
        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.gameObject.transform.localPosition = WeaponOffset;
        }
    }
}