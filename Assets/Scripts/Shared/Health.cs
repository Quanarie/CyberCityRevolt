using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent Dying;
    
    [SerializeField] protected int maxHitPoints;
    protected int currentHitPoints;

    private void Start()
    {
        currentHitPoints = maxHitPoints;
    }

    public void ReceiveDamage(int dmg)
    {
        currentHitPoints -= dmg;
        if (currentHitPoints <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        Dying?.Invoke();
    }
}
