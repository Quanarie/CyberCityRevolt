using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject destroyParticles;
    
    private Rigidbody2D _rb;
    private int _damage;
    
    private Health _target;

    public void Initialize(Vector2 targetPoint, int dmg, float speed)
    {
        if (!TryGetComponent(out _rb))
        {
            Debug.LogError("No Rigidbody2D on bullet: " + gameObject.name);
        }
        
        _rb.velocity = ((Vector3)targetPoint - transform.position).normalized * speed;
        _damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out _target))
        {
            _target.ReceiveDamage(_damage);
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
