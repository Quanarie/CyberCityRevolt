using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private int damage;

    private Health target;

    public void Initialize(Vector3 targetPoint, int dmg, float speed)
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody2D on bullet: " + gameObject.name);
        }
        
        rb.velocity = (targetPoint - transform.position).normalized * speed;
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out target))
        {
            target.ReceiveDamage(damage);
        }
        
        Destroy(gameObject);
    }
}
