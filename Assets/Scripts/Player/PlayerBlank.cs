using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerBlank : MonoBehaviour
{
    [HideInInspector] public UnityEvent StartedBlank;
    
    [SerializeField] private float radius;
    [SerializeField] private float rechargeTime;

    private bool isBlanking;
    private float timeFromLastBlank;

    private void Start()
    {
        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(EndBlankIfStartedRolling);
    }

    private void Update()
    {
        timeFromLastBlank += Time.deltaTime;
    }

    public void OnBlank(InputValue value)
    {
        if (isBlanking || timeFromLastBlank < rechargeTime) return;

        DestroyAllBulletsWithinRadius();
        isBlanking = true;
        StartedBlank?.Invoke();
        timeFromLastBlank = 0f;
    }
    
    private void DestroyAllBulletsWithinRadius()
    {
        foreach (var col in Physics2D.OverlapCircleAll(transform.position, 
                     radius, LayerMask.GetMask("EnemyBullet")))
        {
            if (!col.TryGetComponent(out Bullet bullet)) continue;
            
            bullet.DestroyBullet();
        }
    }

    private void EndBlankIfStartedRolling()
    {
        if (!isBlanking) return;
        
        EndBlanking();
    }

    // !! Called by animator !!
    public void EndBlanking() => isBlanking = false;
}
