using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The weapon must be the child of shooter (player, enemies).
/// Every person that holds a weapon should change its scale to negative while moving to the left
/// (otherwise the rotation of the gun is messed up).
/// Prefab of weapon should be pointing to the right side.
/// Any weapon should be on weapon layer
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    protected CameraMovement cam;
    [SerializeField] protected float cameraShakeDuration;
    [SerializeField] protected float cameraShakeMagnitude;
    
    [SerializeField] protected WeaponInfo info;
    
    protected Bullet[] spawnedBullets;
    protected WeaponInput input;
    
    protected float timeElapsedFromLastShot = 0f;
    protected bool isOnPlayer;

    public bool isDropped { get; protected set; }
    public float GetRechargeTime() => info.rechargeTime;
    public float GetElapsedTime() => timeElapsedFromLastShot;
    
    private void Start()
    {
        if (!transform.parent.TryGetComponent(out input))
        {
            Debug.LogError("No WeaponInput on parent: " + transform.parent.name 
                                                        + " of weapon: " + gameObject.name);
        }
        
        input.Shoot.AddListener(ShootContainer);
        isOnPlayer = transform.parent.TryGetComponent<PlayerMovement>(out _);
        
        if (!Camera.main.TryGetComponent(out cam))
        {
            Debug.LogError("No CameraMovement on Camera.main");
        }

        if (isOnPlayer) return;
        
        if (!transform.parent.TryGetComponent(out Health health))
        {
            Debug.LogError("No Health on weapons parent: " + transform.parent.name);
        }

        health.Dying.AddListener(DropWeapon);
    }

    public virtual void DropWeapon()
    {
        transform.SetParent(null);
        input.Shoot.RemoveListener(ShootContainer);
        isDropped = true;
    }
    
    public void PickupWeapon(Transform parent)
    {
        transform.SetParent(parent);
        isDropped = false;
        Start();
    }

    // FixedUpdate is stopped by setting Time.timeScale to 0f, unlike Update()
    private void FixedUpdate()
    {
        if (isDropped) return;
        
        timeElapsedFromLastShot += Time.fixedDeltaTime;
        RotateWeapon(input.GetTarget());
    }

    private void RotateWeapon(Vector2 whereToAim)
    {
        Vector3 aimDirection = whereToAim - (Vector2)transform.position;
        float aimAngle = Mathf.Atan(aimDirection.y / aimDirection.x) * Mathf.Rad2Deg;

        Transform thisTransform = transform;
        Vector3 weaponScale = thisTransform.localScale;
        Vector3 parentScale = thisTransform.parent.localScale;
        float targetRelative = whereToAim.x - thisTransform.position.x;
        if (parentScale.x > 0 && targetRelative < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(weaponScale.x), weaponScale.y, weaponScale.z);
        }
        else if (parentScale.x < 0 && targetRelative > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(weaponScale.x), weaponScale.y, weaponScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(weaponScale.x), weaponScale.y, weaponScale.z);
        }

        Vector3 rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rot.x, rot.y, aimAngle);
    }
    
    protected virtual void ShootContainer(Vector2 whereToAim)
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        Shoot(whereToAim);
        if (isOnPlayer) cam.Shake(cameraShakeDuration, cameraShakeMagnitude);
        SetLayerBullets();
        OrientBullets();
        timeElapsedFromLastShot = 0;
    }

    /// <summary> Child must stack all bullets that it spawns is SpawnedBullets array </summary> 
    protected abstract void Shoot(Vector2 whereToAim);
    
    protected void SetLayerBullets()
    {
        if (spawnedBullets == null) return;
        
        foreach (var bullet in spawnedBullets)
        {
            if (isOnPlayer)
            {
                bullet.gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
            }
            else
            {
                bullet.gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
            }
        }
    }

    protected void OrientBullets()
    {
        if (spawnedBullets == null) return;
        
        foreach (var bullet in spawnedBullets)
        {
            Vector3 aimDirection = bullet.GetComponent<Rigidbody2D>().velocity;
            float aimAngle = Mathf.Atan(aimDirection.y / aimDirection.x) * Mathf.Rad2Deg;
            
            Vector3 rot = bullet.transform.rotation.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, aimAngle);

            if (aimDirection.x > 0f) continue;
            
            Vector3 bScale = bullet.transform.localScale;
            bullet.transform.localScale = new Vector3(-Mathf.Abs(bScale.x), bScale.y, bScale.z);
        }
    }
}
