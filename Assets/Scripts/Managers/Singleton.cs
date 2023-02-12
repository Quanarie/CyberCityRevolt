using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Main singleton class that gives access to all managers in the game, which
/// must be its children. The information inside these managers is recommended not to be changed,
/// because a bunch of scripts are dependent on them (etc. getting player data, UI objects)
/// Also every script that wants to get information from any manager must do it in Start method, not Awake
/// </summary>
public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    
    public PlayerData PlayerData { get; private set; }
    public DialogueData DialogueData { get; private set; }
    public BulletSpawner BulletSpawner { get; private set; }
    public WeaponInfoManager WeaponInfoManager { get; private set; }
    public StateManager StateManager { get; private set; }
    public NpcManager NpcManager { get; private set; }
    public EnemySpawner EnemySpawner { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        
        PlayerData = GetComponentInChildren<PlayerData>();
        DialogueData = GetComponentInChildren<DialogueData>();
        BulletSpawner = GetComponentInChildren<BulletSpawner>();
        WeaponInfoManager = GetComponentInChildren<WeaponInfoManager>();
        StateManager = GetComponentInChildren<StateManager>();
        NpcManager = GetComponentInChildren<NpcManager>();
        EnemySpawner = GetComponentInChildren<EnemySpawner>();
    }
}