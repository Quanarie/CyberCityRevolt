using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponInput : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Vector2> Shoot;

    /// <returns> Place where weapon should be pointing </returns>
    public abstract Vector2 GetTarget();
}