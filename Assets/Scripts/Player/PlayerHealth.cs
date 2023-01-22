using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Death()
    {
        transform.position = Vector3.zero;
        currentHitPoints = maxHitPoints;
    }
}