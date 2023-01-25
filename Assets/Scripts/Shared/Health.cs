using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [HideInInspector] public UnityEvent Dying;
    
    [SerializeField] protected int maxHitPoints;
    protected int currentHitPoints;

    protected virtual void Start()
    {
        currentHitPoints = maxHitPoints;
    }

    public virtual void ReceiveDamage(int dmg)
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
