using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : Weapon
{
    private SpriteRenderer spriteRenderer;
    private Vector2 nonZeroInput;
    
    private void Start()
    {
        Singleton.Instance.PlayerData.Health.Dying.AddListener(() => gameObject.SetActive(false));
        Singleton.Instance.PlayerData.Health.Respawning.AddListener(() => gameObject.SetActive(true));

        if (!TryGetComponent(out spriteRenderer))
        {
            Debug.LogError("No SpriteRenderer on players weapon");
        }
    }
    
    protected override void Update()
    {
        base.Update();
        RotateWeapon(GetMousePosInWorld());
    }

    private Vector3 GetMousePosInWorld()
    {
        var mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosInWorld = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
        return mousePosInWorld;
    }
    
    // Called by PlayerInput
    private void OnFire(InputValue value)
    {
        if (timeElapsedFromLastShot > rechargeTime)
        {
            Shoot(GetMousePosInWorld());
            timeElapsedFromLastShot = 0f;
        }
    }

    protected override void Shoot(Vector3 whereToAim)
    {
        var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
        bullet.Initialize(whereToAim, damage, bulletSpeed);
    }

    // Rendering the weapon behind the player when they are facing backwards
    private void OnMove(InputValue value)
    {
        var input = value.Get<Vector2>();
        if (input != Vector2.zero)
        {
            nonZeroInput = input;
        }
        
        if (nonZeroInput == Vector2.up)
        {
            spriteRenderer.sortingOrder = Singleton.Instance.PlayerData.Renderer.sortingOrder - 1;
        }
        else
        {
            spriteRenderer.sortingOrder = Singleton.Instance.PlayerData.Renderer.sortingOrder + 1;
        }
    }
}
