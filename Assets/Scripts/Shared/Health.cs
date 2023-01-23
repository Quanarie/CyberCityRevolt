using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int maxHitPoints;
    protected int currentHitPoints;

    private void Start()
    {
        currentHitPoints = maxHitPoints;
    }

    public void ReceiveDamage(int dmg)
    {
        currentHitPoints -= dmg;
        print("received" + gameObject.name);
        if (currentHitPoints <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        
    }
}
