using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

// The weapon must be the child of shooter (player, enemies)
public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int damage;
    [SerializeField] private int bulletSpeed;
    [SerializeField] private float bulletSpawningDistance;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var plPos = Singleton.Instance.PlayerData.Player.transform.position;
            var mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var mousePosInWorldRelative = new Vector3(mousePosInWorld.x - plPos.x, mousePosInWorld.y - plPos.y, 0f);
            var bulletSpawnPos = transform.position + mousePosInWorldRelative.normalized * bulletSpawningDistance;
            var bullet = Instantiate(bulletPrefab, bulletSpawnPos, quaternion.identity).GetComponent<Bullet>();
            bullet.Initialize(mousePosInWorld, damage, bulletSpeed, transform.parent.gameObject);
        } 
    }
}
