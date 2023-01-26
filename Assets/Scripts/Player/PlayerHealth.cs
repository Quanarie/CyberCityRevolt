using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    private bool isDying = false;
    private bool isInvincible = false;

    protected override void Start()
    {
        base.Start();
        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(() => isInvincible = true);
        Singleton.Instance.PlayerData.Movement.EndedRolling.AddListener(() => isInvincible = false);
    }

    public override void ReceiveDamage(int dmg)
    {
        if (isInvincible) return;
        
        base.ReceiveDamage(dmg);
    }

    protected override void Death()
    {
        if (isDying) return;
        
        Singleton.Instance.PlayerData.Input.DeactivateInput();
        base.Death();
        isDying = true;
    }

    // !! Is called by animator in the end of Death animation !!
    public void Respawn()
    {
        Singleton.Instance.PlayerData.Input.ActivateInput();
        transform.position = Vector3.zero;
        currentHitPoints = maxHitPoints;
        isDying = false;
    }
}