using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    private bool isDying = false;
    private bool _isInvincible = false;
    private Vector3 _spawnPoint;

    protected override void Start()
    {
        base.Start();
        _spawnPoint = transform.position;
        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(() => _isInvincible = true);
        Singleton.Instance.PlayerData.Movement.EndedRolling.AddListener(() => _isInvincible = false);
    }

    public override void ReceiveDamage(int dmg)
    {
        if (_isInvincible) return;
        
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}