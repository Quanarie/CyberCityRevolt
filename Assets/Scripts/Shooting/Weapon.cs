using UnityEngine;

/// <summary>
/// The weapon must be the child of shooter (player, enemies).
/// Every person that holds a weapon should change its scale to negative while moving to the left
/// (otherwise the rotation of the gun is messed up).
/// Prefab of weapon should be pointing to the right side.
/// Any weapon should be on weapon layer
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    private CameraMovement cam;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;
    
    [SerializeField] protected WeaponInfo info;
    
    protected Bullet[] spawnedBullets;
    private WeaponInput input;
    
    private float timeElapsedFromLastShot = 0f;
    private bool isOnPlayer;

    public bool isDropped { get; protected set; }
    public float GetRechargeTime() => info.rechargeTime;
    public float GetElapsedTime() => timeElapsedFromLastShot;
    public WeaponInfo GetWeaponInfo() => info;
    
    private void Start()
    {
        if (!transform.parent.TryGetComponent(out input))
        {
            Debug.LogError("No WeaponInput on parent: " + transform.parent.name 
                                                        + " of weapon: " + gameObject.name);
        }
        
        input.Shoot.AddListener(Shoot);
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

    public void DropWeapon()
    {
        StopAllCoroutines();
        PlayerWeaponHandler.StoreWeapon(this);
        transform.SetParent(null);
        input.Shoot.RemoveListener(Shoot);
        isDropped = true;
    }
    
    public void PickupWeapon(Transform parent)
    {
        PlayerWeaponHandler.RemoveWeapon(this);
        transform.SetParent(parent);
        transform.localPosition = input.WeaponOffset;
        isDropped = false;
        Start();
    }

    // FixedUpdate is stopped by setting Time.timeScale to 0f, unlike Update()
    private void FixedUpdate()
    {
        if (isDropped) return;
        
        timeElapsedFromLastShot += Time.fixedDeltaTime;
        RotateWeapon(input.GetWhereToAim());
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
    
    private void Shoot(Vector2 whereToAim)
    {
        if (timeElapsedFromLastShot < info.rechargeTime) return;
        
        SpawnBullets(whereToAim);
        if (isOnPlayer) cam.Shake(cameraShakeDuration, cameraShakeMagnitude);
        Singleton.Instance.BulletManager.ConfigureBullets(spawnedBullets, isOnPlayer);
        timeElapsedFromLastShot = 0;
    }

    /// <summary>
    /// Child must stack all bullets that it spawns is SpawnedBullets array
    /// </summary> 
    protected abstract void SpawnBullets(Vector2 whereToAim);
}