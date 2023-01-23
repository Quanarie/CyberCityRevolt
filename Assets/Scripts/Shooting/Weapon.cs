using UnityEngine;

// The weapon must be the child of shooter (player, enemies)
// Every person that holds a weapon should change its scale to negative while moving to the left
// (otherwise the rotation of the gun is messed up)
// Prefab of weapon should be pointing to the right side 
public class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected int damage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float rechargeTime;

    protected float timeElapsedFromLastShot = 1f;

    protected  virtual void Update()
    {
        timeElapsedFromLastShot += Time.deltaTime;
    }

    protected virtual void RotateWeapon(Vector3 whereToAim)
    {
        var weaponPos = transform.position;
        var aimDirection = whereToAim - weaponPos;
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
    
    protected virtual void Shoot(Vector3 whereToAim)
    {
        
    }
}
