using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponInput : WeaponInput
{
    private Vector3 GetMousePosInWorld()
    {
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
    }
    
    // Called by PlayerInput
    private void OnFire(InputValue value)
    {
        Shoot?.Invoke(GetMousePosInWorld());
    }
    
    public override Vector2 GetTarget() => GetMousePosInWorld();

}