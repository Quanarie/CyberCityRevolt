using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    private List<TalkableNpc> npcs;
    
    private void Start()
    {
        npcs = new();
        foreach (TalkableNpc npc in FindObjectsByType<TalkableNpc>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            npcs.Add(npc);
        }
    }

    public TalkableNpc FindClosestNpcToPlayer()
    {
        Vector3 plPos = Singleton.Instance.PlayerData.Player.transform.position;
        TalkableNpc closestNpc = npcs[0];
        for (int i = 0; i < npcs.Count; i++)
        {
            if (Vector3.Distance(npcs[i].transform.position, plPos) < 
                Vector3.Distance(closestNpc.transform.position, plPos))
            {
                closestNpc = npcs[i];
            }
        }

        return closestNpc;
    }
}