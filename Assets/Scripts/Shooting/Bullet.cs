using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject destroyParticles;
    
    private Rigidbody2D rb;
    private int damage;
    private bool didDamage = false;

    private Health _target;

    public void Initialize(Vector2 targetPoint, int dmg, float speed)
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody2D on bullet: " + gameObject.name);
        }
        
        rb.velocity = ((Vector3)targetPoint - transform.position).normalized * speed;
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out _target) && !didDamage)
        {
            didDamage = true;
            _target.ReceiveDamage(damage);
            DestroyBullet();
            return;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Collision"))
        {
            DestroyBullet();
        }
    }
    
    public void DestroyBullet()
    {
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
