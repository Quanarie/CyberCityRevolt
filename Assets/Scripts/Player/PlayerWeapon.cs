using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    protected override void Update()
    {
        base.Update();
        
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosInWorld = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
        
        RotateWeapon(mousePosInWorld);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot(mousePosInWorld);
        }
    }
}
