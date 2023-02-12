using UnityEngine;

public abstract class ConfigurableWeapon : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected int damage;
    [SerializeField] protected float bulletSpeed;

    [SerializeField] private float period;
    [SerializeField] private float elapsedSinceLastShot;

    private void Update()
    {
        elapsedSinceLastShot += Time.deltaTime;
        if (elapsedSinceLastShot < period) return;

        SpawnBullets();
        elapsedSinceLastShot = 0f;
    }
    
    protected abstract void SpawnBullets();
}