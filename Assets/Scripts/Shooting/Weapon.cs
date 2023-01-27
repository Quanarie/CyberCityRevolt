using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The weapon must be the child of shooter (player, enemies).
/// Every person that holds a weapon should change its scale to negative while moving to the left
/// (otherwise the rotation of the gun is messed up).
/// Prefab of weapon should be pointing to the right side.
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    public UnityEvent Shot;
    
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected int damage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] private float rechargeTime;
    
    private float timeElapsedFromLastShot = 1f;
    protected Bullet[] spawnedBullets;
    protected WeaponInput input;
    private bool isOnPlayer;

    private void Start()
    {
        if (!transform.parent.TryGetComponent(out input))
        {
            Debug.LogError("No WeaponInput on: " + transform.parent.name);
        }
        
        input.Shoot.AddListener(ShootContainer);
        isOnPlayer = transform.parent.TryGetComponent<PlayerMovement>(out _);
    }
    
    // FixedUpdate is stopped by setting Time.timeScale to 0f, unlike Update()
    private void FixedUpdate()
    {
        timeElapsedFromLastShot += Time.fixedDeltaTime;
        RotateWeapon(input.GetTarget());
    }

    private void RotateWeapon(Vector2 whereToAim)
    {
        var aimDirection = whereToAim - (Vector2)transform.position;
        var aimAngle = Mathf.Atan(aimDirection.y / aimDirection.x) * Mathf.Rad2Deg;

        var thisTransform = transform;
        var weaponScale = thisTransform.localScale;
        var parentScale = thisTransform.parent.localScale;
        var targetRelative = whereToAim.x - thisTransform.position.x;
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

        var rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rot.x, rot.y, aimAngle);
    }
    
    private void ShootContainer(Vector2 whereToAim)
    {
        if (timeElapsedFromLastShot < rechargeTime) return;
        
        Shoot(whereToAim);
        Shot?.Invoke();
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

    private void OrientBullets()
    {
        if (spawnedBullets == null) return;
        
        foreach (var bullet in spawnedBullets)
        {
            var aimDirection = bullet.GetComponent<Rigidbody2D>().velocity;
            var aimAngle = Mathf.Atan(aimDirection.y / aimDirection.x) * Mathf.Rad2Deg;
            
            var rot = bullet.transform.rotation.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, aimAngle);

            if (aimDirection.x > 0f) continue;
            
            var bScale = bullet.transform.localScale;
            bullet.transform.localScale = new Vector3(-Mathf.Abs(bScale.x), bScale.y, bScale.z);
        }
    }
}
