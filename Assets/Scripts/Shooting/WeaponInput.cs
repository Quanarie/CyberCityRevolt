using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponInput : MonoBehaviour
{
    /// <summary>
    /// Invoke Shoot when enemy shoudl try to shoot
    /// </summary>
    [HideInInspector] public UnityEvent Shoot;

    /// <returns> Place where weapon should be pointing </returns>
    public abstract Vector2 GetTarget();
}