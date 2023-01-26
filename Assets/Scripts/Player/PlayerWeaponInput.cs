using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponInput : WeaponInput
{
    private Vector3 GetMousePosInWorld()
    {
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosInWorld = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
        return mousePosInWorld;
    }
    
    // Called by PlayerInput
    private void OnFire(InputValue value)
    {
        Shoot?.Invoke(GetMousePosInWorld());
    }
    
    public override Vector2 GetTarget() => GetMousePosInWorld();

}