using UnityEngine;

public class PlayerHealth : Health
{
    private bool isDying = false;
    
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